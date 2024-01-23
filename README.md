<div align="center">
<img src="https://projectzomboid.com/blog/content/uploads/2022/10/spiffoguard_transparent1.png" alt="대표 이미지"   height="200" />
<br/ >
<br/ >

# PzomPatch
이 프로그램은 프로젝트 좀보이드 간편 패치 윈도우 데스크톱 어플리케이션입니다.

[다운로드](https://github.com/huhu0327/PzomPatch/releases/latest/download/PzomPatch.exe)
</div>

## 사용법
1. 스팀/좀보이드 경로 지정
2. 램 불러오기
3. 원하는 램 크기 할당
4. 닉네임/서버주소 입력 후 적용
5. 게임 실행
6. (No Mo Culling, BetterFPS 모드가 있다면) 서버 모드 다운로드 이후 모드 패치

## 기능
- 클라이언트 메모리 사이즈 조정 ( ProjectZomboid64.bat , ProjectZomboid64.json )
- **좀비 증발 Fix**(No Mo Culling)와 **더나은FPS**(BetterFPS)모드 패치
- ServerListSteam 서버 추가

## 스크린샷

<p align="center">
<img src="https://github.com/huhu0327/new/assets/28612967/c4fdbc94-7b29-4898-bd2f-c3f0c03d9294">
</p>

## 개발 환경
- Lang
  - C# / .NET8  
- Framework
  - AvaloniaUI
- IDE
  - Rider IDE  
- nuget
  - CommunityToolkit.Mvvm  
  - Microsoft.Extensions.Dependencyinjection
  - Serilog
  - Salaros.ConfigParser

## 개선 사항
코드 리팩토링  
테스트코드 작성  
Github Action -> CI / CD 환경 구축  
GUI 개선  
사용법 doc으로 분리