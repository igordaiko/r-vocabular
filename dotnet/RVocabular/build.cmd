rmdir /s/q out

dotnet publish -c Release -p:DebugType=None -p:DebugSymbols=false -p:PublishProfile=default -p:TargetRuntimeIdentifier=linux-x64 ./RVocabular.Host -o ./out

cd out && tar -czvf ../r-vocabular-host.tar.gz ./* && cd..

cd ..\..\keys

pscp -i "rt-key.ppk" ../dotnet/RVocabular/r-vocabular-host.tar.gz ubuntu@ec2-18-156-78-200.eu-central-1.compute.amazonaws.com:/r-vocabular-host

ssh -i "rt-key.pem" ubuntu@ec2-18-156-78-200.eu-central-1.compute.amazonaws.com "cd /r-vocabular-host; sudo ./run.sh"
