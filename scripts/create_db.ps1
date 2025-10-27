# Create LocalDB and run init_db.sql (PowerShell)
$cmd = @"
SqlLocalDB info
"@
Write-Host "Run init_db.sql manually or use your DB tool to create the Users table."
