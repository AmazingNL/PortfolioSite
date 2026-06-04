-- ============================================================================
-- Starter "Upcoming Projects" roadmap — run AFTER schema.sql.
-- Supabase Dashboard → SQL Editor → New query → paste → Run.
--
-- These are editable starters tailored to your stack — tweak the wording,
-- statuses and dates here (re-run to apply) or manage them live from /admin.
-- Statuses understood by the site's styling: 'Planned', 'In progress', 'Researching'.
-- ============================================================================

-- Unique title makes this re-runnable: re-running UPDATES each row in place.
create unique index if not exists upcoming_projects_title_key on public.upcoming_projects (title);

insert into public.upcoming_projects (title, description, status, target_date, sort_order) values
    ('Launch this portfolio',
     'Deploy this Blazor WebAssembly + Supabase portfolio publicly, tag my best repositories, and polish the content.',
     'In progress', 'Q3 2026', 1),

    ('Recipe & Meal Planner',
     'A full-stack ASP.NET Core + Blazor app backed by Supabase to plan weekly meals and auto-generate a shopping list.',
     'Planned', 'Q4 2026', 2),

    ('Secure REST API starter',
     'A clean ASP.NET Core Web API template with JWT auth, role-based access and OpenAPI docs, containerised with Docker.',
     'Planned', 'Q4 2026', 3),

    ('NoSQL event booking',
     'Rebuild an event-booking service on MongoDB to compare the design against my SQL implementations.',
     'Researching', '2027', 4),

    ('Cross-platform mobile app (.NET MAUI)',
     'Explore mobile development by rebuilding one of my web apps as a .NET MAUI client sharing a common API.',
     'Researching', '2027', 5)
on conflict (title) do update set
    description = excluded.description,
    status      = excluded.status,
    target_date = excluded.target_date,
    sort_order  = excluded.sort_order;
