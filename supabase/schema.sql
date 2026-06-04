-- ============================================================================
-- Portfolio site — Supabase schema
-- Run this in your Supabase project: Dashboard → SQL Editor → New query → paste → Run.
-- ============================================================================

-- ---- Profile (single row, your "About me") --------------------------------
create table if not exists public.profile (
    id           bigint primary key default 1,
    full_name    text not null default '',
    headline     text not null default '',
    bio          text not null default '',
    location     text,
    email        text,
    github_url   text,
    linkedin_url text,
    avatar_url   text
);

-- Seed the single profile row so the admin "Save profile" upsert has a target.
insert into public.profile (id, full_name, headline, bio, github_url)
values (1, 'Amazinggrace Iruoma', 'Software Developer',
        'I build web applications with C#, Blazor and modern tooling.',
        'https://github.com/AmazingNL')
on conflict (id) do nothing;

-- ---- Skills ----------------------------------------------------------------
create table if not exists public.skills (
    id         bigint generated always as identity primary key,
    name       text not null,
    category   text not null default 'General',
    level      int  not null default 3,
    sort_order int  not null default 0
);

-- Unique skill names so the seed file's upsert is re-runnable.
create unique index if not exists skills_name_key on public.skills (name);

-- ---- Upcoming projects -----------------------------------------------------
create table if not exists public.upcoming_projects (
    id          bigint generated always as identity primary key,
    title       text not null,
    description text not null default '',
    status      text not null default 'Planned',
    target_date text,
    sort_order  int  not null default 0
);

-- Unique titles so the upcoming-projects seed file's upsert is re-runnable.
create unique index if not exists upcoming_projects_title_key on public.upcoming_projects (title);

-- ---- Contact messages ------------------------------------------------------
create table if not exists public.messages (
    id         bigint generated always as identity primary key,
    name       text not null,
    email      text not null,
    message    text not null,
    created_at timestamptz not null default now()
);

-- ============================================================================
-- Row Level Security
--   * profile / skills / upcoming_projects: anyone can READ; only authenticated
--     users (you, logged in) can write.
--   * messages: anyone can INSERT (the contact form); only authenticated users
--     can READ them (the admin inbox).
-- ============================================================================

alter table public.profile           enable row level security;
alter table public.skills            enable row level security;
alter table public.upcoming_projects enable row level security;
alter table public.messages          enable row level security;

-- Public read on content tables
create policy "public read profile"  on public.profile           for select using (true);
create policy "public read skills"   on public.skills            for select using (true);
create policy "public read upcoming" on public.upcoming_projects for select using (true);

-- Authenticated full write on content tables
create policy "auth write profile"  on public.profile
    for all to authenticated using (true) with check (true);
create policy "auth write skills"   on public.skills
    for all to authenticated using (true) with check (true);
create policy "auth write upcoming" on public.upcoming_projects
    for all to authenticated using (true) with check (true);

-- Messages: anyone may submit, only authenticated may read
create policy "anyone insert messages" on public.messages
    for insert to anon, authenticated with check (true);
create policy "auth read messages"     on public.messages
    for select to authenticated using (true);
