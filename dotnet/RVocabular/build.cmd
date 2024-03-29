rmdir /s/q out

dotnet publish -c Release -p:DebugType=None -p:DebugSymbols=false -p:PublishProfile=default -p:TargetRuntimeIdentifier=linux-x64 ./RVocabular.Host -o ./out

cd out && tar -czvf ../r-vocabular-host.tar.gz ./* && cd..

cd ..\..\keys

pscp -i private_key ../dotnet/RVocabular/r-vocabular-host.tar.gz ${USER_NAME}@${HOST_NAME_1}:/r-vocabular-host
pscp -i private_key ../dotnet/RVocabular/r-vocabular-host.tar.gz ${USER_NAME}@${HOST_NAME_2}:/r-vocabular-host

ssh -i private_key ${USER_NAME}@${HOST_NAME_1} "cd /r-vocabular-host; sudo ./run.sh"
ssh -i private_key ${USER_NAME}@${HOST_NAME_2} "cd /r-vocabular-host; sudo ./run.sh"
