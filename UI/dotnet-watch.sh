#!/bin/bash

set -o allexport
source .env
set +o allexport
dotnet watch run --urls "https://localhost:7213"