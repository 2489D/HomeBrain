module Domain
open System

type Name20 = Name20 of string

type User = {
  Id: Guid
  Name: Name20
}

type File =
  | Photo
  | PDF

type Submission = {
  Id: Guid
  File: File array
  Date: DateTime
}

// TODO: Not useful for pattern matching
type Student = Student of User
type Host = Host of User

type String40 = String40 of string

type RoomStatus =
  | Waiting
  | On
  | End

// Shares server time
type Room = {
  Id: Guid
  Title: String40
  Hosts: Host array
  Students: Student array
  Status: RoomStatus
}

type String300 = String300 of string

// FIXME
type ResponseFn = unit -> bool
type ExamRequest = Student * Host array * String300 * ResponseFn
type ExamResponse = Host * Student * String300 * bool

let tryName s =
  if s |> String.length > 20 then None
  else Some s

let user name =
  tryName name
  |> Option.map (fun n -> {
    Id = Guid.NewGuid ();
    Name = Name20 n;
  })