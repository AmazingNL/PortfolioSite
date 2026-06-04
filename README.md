# Portfolio Site

A personal portfolio built with **Blazor WebAssembly (.NET 10)** and **Supabase**.

## Sections

- **Projects** — public GitHub repos tagged with the `portfolio` topic, fetched live from the GitHub API.
- **About Me** — bio + links, editable from the admin page (stored in Supabase).
- **Skills** — grouped skill list with proficiency meters (Supabase).
- **Upcoming Projects** — a roadmap timeline (Supabase).
- **Contact** — a form that saves messages to Supabase.
- **Admin** (`/admin`) — log in with Supabase Auth to edit content and read messages.

## Setup

### 1. Create a Supabase project
Go to [supabase.com](https://supabase.com), create a free project.

### 2. Run the database schema
In the Supabase dashboard: **SQL Editor → New query**, paste the contents of
[`supabase/schema.sql`](supabase/schema.sql), and **Run**. This creates the tables
and Row Level Security policies (public read, you-only write, public contact submits).

### 3. Create your admin user
Supabase dashboard → **Authentication → Users → Add user** (email + password).
That's the account you'll log in with at `/admin`.

### 4. Add your keys
Edit [`wwwroot/appsettings.json`](wwwroot/appsettings.json):

```json
{
  "GitHub": { "Username": "AmazingNL", "Topic": "portfolio" },
  "Supabase": {
    "Url": "https://YOUR_PROJECT_REF.supabase.co",
    "AnonKey": "YOUR_SUPABASE_ANON_KEY"
  }
}
```

Find these under Supabase **Settings → API** (Project URL and the `anon` `public` key).
The anon key is *meant* to be public — security is enforced by the RLS policies.

### 5. Feature your repos
On GitHub, open each repo you want shown → the ⚙️ next to **About** → add the
topic **`portfolio`**. Only tagged, non-fork, non-archived repos appear.

## Run locally

```bash
dotnet run
```

Then open the URL it prints (e.g. http://localhost:5182).

## Deploy

Because it's WebAssembly, it publishes to static files:

```bash
dotnet publish -c Release
```

Upload `bin/Release/net10.0/publish/wwwroot` to any static host —
GitHub Pages, Netlify, Cloudflare Pages, or Azure Static Web Apps.

> For client-side routing (so `/admin` works on refresh), configure the host to
> fall back to `index.html` (e.g. a `404.html` copy on GitHub Pages, or a
> rewrite rule on Netlify/Cloudflare).
