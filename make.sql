CREATE TABLE blog (
	"BLOG_ID"	INTEGER NOT NULL,
	"AUTHOR"	TEXT,
	"TITLE"	TEXT,
	"POST_DATE"	INTEGER,
	"SUMMARY"	TEXT,
	"BODY"	TEXT,
	PRIMARY KEY("BLOG_ID" AUTOINCREMENT)
);

CREATE TABLE "admin_config" (
	"ADMIN_USER"	TEXT DEFAULT 'admin',
	"KEY"	TEXT DEFAULT '',
	"KEY_SET"	INTEGER DEFAULT 0,
	PRIMARY KEY("ADMIN_USER")
);
INSERT INTO "admin_config" ("ADMIN_USER", "KEY", "KEY_SET") VALUES ('admin', '', '0');

CREATE TABLE "admin_session" (
	"SESSION_ID"	TEXT NOT NULL,
	"SESSION_TIME"	INTEGER,
	PRIMARY KEY("SESSION_ID")
);
