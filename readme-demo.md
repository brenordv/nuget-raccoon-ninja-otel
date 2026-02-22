
Postgres:
```tsql
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

CREATE TABLE IF NOT EXISTS watched (
    letterboxd_uri   TEXT      NOT NULL,
    watch_date       DATE      NOT NULL,
    title            TEXT      NOT NULL,
    release_year     SMALLINT  NOT NULL
        CHECK (release_year BETWEEN 1878 AND 2100),
    cache_id         UUID      NULL,
    genres           TEXT[]    NULL,

    CONSTRAINT letterboxd_watchlist_pk
        PRIMARY KEY (letterboxd_uri)
);
```

```tsql
INSERT INTO watched (letterboxd_uri, watch_date, title, release_year, genres, cache_id)
VALUES
    ('/film/the-shawshank-redemption/', '2025-01-15', 'The Shawshank Redemption', 1994, ARRAY['Drama','Crime'], 'a1b2c3d4-e5f6-7890-abcd-ef1234567890'),
    ('/film/parasite-2019/', '2025-02-03', 'Parasite', 2019, ARRAY['Thriller','Drama','Comedy'], 'b2c3d4e5-f6a7-8901-bcde-f12345678901'),
    ('/film/spirited-away/', '2025-02-10', 'Spirited Away', 2001, ARRAY['Animation','Fantasy','Adventure'], 'c3d4e5f6-a7b8-9012-cdef-123456789012'),
    ('/film/the-godfather/', '2025-01-28', 'The Godfather', 1972, ARRAY['Crime','Drama'], 'd4e5f6a7-b8c9-0123-defa-234567890123'),
    ('/film/everything-everywhere-all-at-once/', '2025-02-18', 'Everything Everywhere All at Once', 2022, ARRAY['Action','Adventure','Comedy','Sci-Fi'], 'e5f6a7b8-c9d0-1234-efab-345678901234');
```

Sql Server:
```tsql
IF NOT EXISTS (
    SELECT * FROM sys.tables WHERE name = 'watched'
)
CREATE TABLE watched (
                         letterboxd_uri   NVARCHAR(200)        NOT NULL,
                         watch_date       DATE                 NOT NULL,
                         title            NVARCHAR(MAX)        NOT NULL,
                         release_year     SMALLINT             NOT NULL
                             CHECK (release_year BETWEEN 1878 AND 2100),
                         cache_id         UNIQUEIDENTIFIER     NULL,
                         genres           NVARCHAR(MAX)        NULL,

                         CONSTRAINT letterboxd_watchlist_pk
                             PRIMARY KEY (letterboxd_uri)
);
```


```tsql
INSERT INTO watched (letterboxd_uri, watch_date, title, release_year, genres, cache_id)
VALUES
    ('/film/the-shawshank-redemption/', '2025-01-15', 'The Shawshank Redemption', 1994, '["Drama","Crime"]', 'a1b2c3d4-e5f6-7890-abcd-ef1234567890'),
    ('/film/parasite-2019/', '2025-02-03', 'Parasite', 2019, '["Thriller","Drama","Comedy"]', 'b2c3d4e5-f6a7-8901-bcde-f12345678901'),
    ('/film/spirited-away/', '2025-02-10', 'Spirited Away', 2001, '["Animation","Fantasy","Adventure"]', 'c3d4e5f6-a7b8-9012-cdef-123456789012'),
    ('/film/the-godfather/', '2025-01-28', 'The Godfather', 1972, '["Crime","Drama"]', 'd4e5f6a7-b8c9-0123-defa-234567890123'),
    ('/film/everything-everywhere-all-at-once/', '2025-02-18', 'Everything Everywhere All at Once', 2022, '["Action","Adventure","Comedy","Sci-Fi"]', 'e5f6a7b8-c9d0-1234-efab-345678901234');
```