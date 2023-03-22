
open System

type IMyDate =
    abstract member GetDateTime: unit -> DateTime
    
type MyDate() =
    interface IMyDate with
        member _.GetDateTime() =
            DateTime.Now
            
type IMyHttpClient =
    abstract member GetRemoteData: int -> (int * string)

type MyHttpClient() =
    interface IMyHttpClient with
        member _.GetRemoteData id =
            let users =
                [ 1, "kito"
                  2, "mnebes"
                  3, "fphilipe" ]
                |> dict
            
            (id, users[id])
            
type IConfiguration =
    abstract member ConnectionString: string

type Configuration() =
    interface IConfiguration with
        member _.ConnectionString = "localhost"
            
type IDatabase =
    abstract member Save: (int * string) -> unit
    
type Database(config: IConfiguration) =
    interface IDatabase with
        member _.Save user =
            let id, username = user
            printfn $"Saving user with id: '%i{id}' and username: '%s{username}' to database with connectionstring: '%s{config.ConnectionString}'"
            ()
            
type IMyService =
    abstract member DoThing: int -> string
    
type MyService(db: IDatabase, http: IMyHttpClient, date: IMyDate) =
    interface IMyService with
        member _.DoThing id =
            let user = http.GetRemoteData id
            let currentTime = date.GetDateTime().ToShortTimeString()
            
            db.Save user
            
            $"Saved user: '%s{snd user}' into db at: '%s{currentTime}'"



let date = new MyDate()
let httpClient = new MyHttpClient()
let config = new Configuration()
let db = new Database(config)
let myService = new MyService(db, httpClient, date) :> IMyService

let response = myService.DoThing 1

printfn $"Response is: '%s{response}'"