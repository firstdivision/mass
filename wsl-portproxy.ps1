# ============================
# WSL Port Forward Script
# Distro: Debian
# ============================

$DistroName = "Debian"
$ListenPort = 7213
$ListenAddress = "0.0.0.0"

Write-Host "Updating WSL port forwarding for ${DistroName} on port ${ListenPort}..."

# Get WSL IP
$WslIP = wsl -d $DistroName hostname -I
$WslIP = $WslIP.Trim().Split(" ")[0]

if (-not $WslIP) {
    Write-Error "Could not determine WSL IP"
    exit 1
}

Write-Host "WSL IP detected: ${WslIP}"

# Remove existing portproxy rule (ignore errors if it doesn't exist)
netsh interface portproxy delete v4tov4 `
    listenaddress=$ListenAddress `
    listenport=$ListenPort 2>$null

# Add new portproxy rule
netsh interface portproxy add v4tov4 `
    listenaddress=$ListenAddress listenport=$ListenPort `
    connectaddress=$WslIP connectport=$ListenPort

Write-Host "Port proxy updated: ${ListenAddress}:${ListenPort} -> ${WslIP}:${ListenPort}"

# # Ensure firewall rule exists
# $FirewallRuleName = "WSL Port $ListenPort"

# if (-not (Get-NetFirewallRule -DisplayName $FirewallRuleName -ErrorAction SilentlyContinue)) {
#     New-NetFirewallRule `
#         -DisplayName $FirewallRuleName `
#         -Direction Inbound `
#         -Protocol TCP `
#         -LocalPort $ListenPort `
#         -Action Allow | Out-Null
#     Write-Host "Firewall rule created."
# }
# else {
#     Write-Host "Firewall rule already exists."
# }

Write-Host "Done."
