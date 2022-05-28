::modify this path to use appropriate version of MSBuild:
set tool=%windir%\Microsoft.NET\Framework64\v3.5\MSBuild.exe
set solution=HandwritingRecognition.sln

%tool% %solution% /p:Configuration=Debug
%tool% %solution% /p:Configuration=Release
