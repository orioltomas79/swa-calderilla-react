wt -w 0 nt --title "API" --suppressApplicationTitle --tabColor "#6B8E35" -d .\swa-calderilla-api\Calderilla.Api powershell.exe -NoExit -Command "func host start"
wt -w 0 nt --title "FE" --suppressApplicationTitle --tabColor "#8E4535" -d .\swa-calderilla-fe powershell.exe -NoExit -Command "pnpm dev"
wt -w 0 nt --title "SWA" --suppressApplicationTitle --tabColor "#8EEEEE" -d . powershell.exe -NoExit -Command "swa start"
