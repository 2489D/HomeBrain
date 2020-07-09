module Domain
open System

type Name20 = Name20 of string

type File =
  | Photo
  | PDF
  | Essay

type Submission = {
  Id: Guid
  File: File array
  Date: DateTime
}

type Person = {
  Id: Guid
  Name: Name20
}

type User =
  | Student of Person
  | Host of Person

type String40 = String40 of string

type RoomStatus =
  | Waiting
  | On
  | End

// Shares server time
type Room = {
  Id: Guid
  Title: String40
  Hosts: User array
  Students: User array
  Status: RoomStatus
}

type String300 = String300 of string

type Message = String300

let tryName s =
  if s |> String.length > 20 then None
  else Some s

let user name =
  tryName name
  |> Option.map (fun n -> {
    Id = Guid.NewGuid ();
    Name = Name20 n;
  })