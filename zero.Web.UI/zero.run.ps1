
param(
  # get, don't execute
  [Parameter(Mandatory=$false)]
  [Alias("g")]
  [switch] $get = $false,

  # execute a command
  [Parameter(Mandatory=$false, ValueFromRemainingArguments=$true)]
  [String[]]
  $command
)

npx vue-cli-service serve