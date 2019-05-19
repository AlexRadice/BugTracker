sqllocaldb create bugtracker
sqllocaldb start bugtracker
sqlcmd -S (LocalDb)\bugtracker -E -i CreateSchema.sql