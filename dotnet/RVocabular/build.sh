rm -rf out

dotnet publish -c Release -p:DebugType=None -p:DebugSymbols=false -p:PublishProfile=default -p:TargetRuntimeIdentifier=linux-x64 ./RVocabular.Host -o ./out

cd out && tar -czvf ../r-vocabular-host.tar.gz ./* && cd ..

scp -o StrictHostKeyChecking=no -i "private_key" ./r-vocabular-host.tar.gz ${USER_NAME}@${HOST_NAME_1}:/r-vocabular-host
scp -o StrictHostKeyChecking=no -i "private_key" ./r-vocabular-host.tar.gz ${USER_NAME}@${HOST_NAME_2}:/r-vocabular-host

# echo $USER_NAME
# echo $HOST_NAME_1
# echo $HOST_NAME_2

ssh -o StrictHostKeyChecking=no -i "private_key" ${USER_NAME}@${HOST_NAME_1} "cd /r-vocabular-host; sudo ./run.sh"
ssh -o StrictHostKeyChecking=no -i "private_key" ${USER_NAME}@${HOST_NAME_2} "cd /r-vocabular-host; sudo ./run.sh"
