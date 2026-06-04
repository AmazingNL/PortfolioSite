-- ============================================================================
-- Portfolio seed data — run AFTER schema.sql.
-- Supabase Dashboard → SQL Editor → New query → paste → Run.
--
-- SKILLS are derived from your actual GitHub repositories.
-- PROFILE is a DRAFT — edit the bio / location / links / email below
-- (or change them later from the /admin page).
-- ============================================================================

-- ---- Profile (About me) ----------------------------------------------------
insert into public.profile (id, full_name, headline, bio, location, email, github_url, linkedin_url, avatar_url)
values (
    1,
    'Amazinggrace Iruoma',
    'Software Developer',
    'I''m a software developer who enjoys building full-stack web applications. '
        || 'My projects span C# / ASP.NET, PHP and Java — from a restaurant ordering '
        || 'system to event-booking and fashion web apps — backed by both SQL and '
        || 'NoSQL databases and containerised with Docker. I like turning ideas into '
        || 'working, well-structured software.',
    'Netherlands',                                  -- TODO: confirm/replace
    'amazinggraceiruomaNL@gmail.com',               -- TODO: confirm/replace
    'https://github.com/AmazingNL',
    null,                                           -- TODO: add your LinkedIn URL
    null                                            -- TODO: add an avatar image URL (optional)
)
on conflict (id) do update set
    full_name    = excluded.full_name,
    headline     = excluded.headline,
    bio          = excluded.bio,
    location     = excluded.location,
    email        = excluded.email,
    github_url   = excluded.github_url,
    linkedin_url = excluded.linkedin_url,
    avatar_url   = excluded.avatar_url;

-- ---- Skills (derived from your repositories) -------------------------------
-- level is 1-5 proficiency, used for the little meter on the site.
-- A unique index on name makes this re-runnable: re-running UPDATES each skill's
-- category/level/order in place instead of creating duplicates.
create unique index if not exists skills_name_key on public.skills (name);

insert into public.skills (name, category, level, sort_order) values
    -- Languages
    ('C#',          'Languages',        5, 1),
    ('Java',        'Languages',        4, 2),
    ('PHP',         'Languages',        4, 3),
    ('JavaScript',  'Languages',        3, 4),
    ('SQL',         'Languages',        4, 5),
    ('HTML & CSS',  'Languages',        4, 6),
    -- Frameworks
    ('ASP.NET Core','Frameworks',       4, 7),
    ('.NET',        'Frameworks',       4, 8),
    ('Blazor',      'Frameworks',       3, 9),
    -- Databases
    ('SQL Server',  'Databases',        4, 10),
    ('MongoDB',     'Databases',        3, 11),
    -- Tools & DevOps
    ('Docker',      'Tools & DevOps',   3, 12),
    ('Git & GitHub','Tools & DevOps',   4, 13),
    ('PowerShell',  'Tools & DevOps',   3, 14),
    ('Visual Studio','Tools & DevOps',  4, 15)
on conflict (name) do update set
    category   = excluded.category,
    level      = excluded.level,
    sort_order = excluded.sort_order;
