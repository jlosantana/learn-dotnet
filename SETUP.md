sudo add-apt-repository ppa:dotnet/backports

sudo apt-get update && \
  sudo apt-get install -y dotnet-sdk-9.0

https://www.nuget.org/

dotnet add package PasswordGenerator --version 2.1.0