#!/bin/bash

set -o allexport
source ../.env
set +o allexport
dotnet watch run --urls "https://0.0.0.0:7213"