module Tests

open System
open FakeItEasy
open Moq
open Moq.AutoMock
open Program
open Xunit

[<Fact>]
let ``Fake it easy simple example`` () =
    let dateFake = A.Fake<IMyDate>()
    let httpFake = A.Fake<IMyHttpClient>()
    let dbFake = A.Fake<IDatabase>()
    let myService = MyService(dbFake, httpFake, dateFake) :> IMyService
    
    let response = myService.DoThing 1
    
    Assert.Equal(response, "Saved user: '' into db at: '12:00 AM'")
    
[<Fact>]
let ``Fake it easy with mocking`` () =
    let dateFake = A.Fake<IMyDate>()
    let httpFake = A.Fake<IMyHttpClient>()
    let dbFake = A.Fake<IDatabase>()
    
    A.CallTo(fun _ -> dateFake.GetDateTime()).Returns(DateTime.Parse("2020-03-01T16:42:00")) |> ignore
    A.CallTo(fun _ -> httpFake.GetRemoteData(A.Ignored)).Returns((42, "fakeUser")) |> ignore
    
    let myService = MyService(dbFake, httpFake, dateFake) :> IMyService
    
    let response = myService.DoThing 1
    
    Assert.Equal(response, "Saved user: 'fakeUser' into db at: '4:42 PM'")
    
    A.CallTo(fun _ -> dbFake.Save(A.Ignored)).MustHaveHappenedOnceExactly()
    
[<Fact>]
let ``Moq example`` () =
    let dateMock = new Mock<IMyDate>()
    let httpMock = new Mock<IMyHttpClient>()
    let dbMock = new Mock<IDatabase>()
    
    httpMock.Setup(fun http -> http.GetRemoteData(It.IsAny<int>())).Returns((42, "mock user")) |> ignore
    let myService = new MyService(dbMock.Object, httpMock.Object, dateMock.Object) :> IMyService
    
    let response = myService.DoThing 1
    Assert.Equal(response, "Saved user: 'mock user' into db at: '12:00 AM'")
    
    dbMock.Verify((fun db -> db.Save(It.IsAny<int * string>())), Times.Once())
    
[<Fact>]
let ``Automock example`` () =
    let mocker = new AutoMocker()
    
    let httpMock = mocker.GetMock<IMyHttpClient>()
    httpMock.Setup(fun http -> http.GetRemoteData(It.IsAny<int>())).Returns((42, "mock user")) |> ignore
    
    let myService = mocker.CreateInstance<MyService>() :> IMyService
    
    let response = myService.DoThing 1
    Assert.Equal(response, "Saved user: 'mock user' into db at: '12:00 AM'")