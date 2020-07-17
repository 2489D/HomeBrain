module HomeBrain.Domain
open System
open FSharp.Collections

type Name20 = Name20 of string

type File =
  | Photo
  | PDF

type Paper = File list

/// TODO
/// 1. How to represent file in F#
/// 2. How to represent html forms or relevant data in this type `File`
type SubmissionData =
  | File
  | Form

[<NoEquality; NoComparison>]
type Submission = {
  Data: SubmissionData list
  Date: DateTimeOffset
}

module User =
  type StudentGuid = Guid
  type StudentId = IdString of string

  [<NoEquality; NoComparison>]
  type StudentData = {
    Id: StudentGuid
    Name: Name20
    StdId: StudentId
    Submissions: Submission list
  }

  type HostGuid = Guid

  [<NoEquality; NoComparison>]
  type HostData = {
    Id: HostGuid
    Name: Name20
  }
  
  type Student = StudentData
  type Host = HostData

  type User =
    | Student of Student
    | Host of Host


module Message =
  type MsgString300 = MsgString300 of string
  type RequestFn = unit -> unit // FIXME

  module MsgString =
    let create msg =
      if msg |> String.length > 300 then None
      else Some (MsgString300 msg)

  type MessageContent =
    | MsgString of MsgString300
    | RequestFn

  type MessageFromStudentToHost = {
    Sender: User.Student
    Receivers: User.Host list
    Content: MessageContent
    Time: DateTimeOffset
  }

  type MessageFromHostToStudent = {
    Sender: User.Host
    Receivers: User.Student list
    Content: MessageContent
    Time: DateTimeOffset
  }

  type MessageFromHostToHost = {
    Sender: User.Host
    Receivers: User.Host list
    Content: MessageContent
    Time: DateTimeOffset
  }

  type Notice = {
    Sender: User.Host
    Receiver: User.User list
    Content: MessageContent
    Time: DateTimeOffset
  }

  type Message =
    | MessageFromStudentToHost of MessageFromStudentToHost
    | MessageFromHostToStudent of MessageFromHostToStudent
    | MessageFromHostToHost of MessageFromHostToHost
    | Notice of Notice


type RoomGuid = Guid
type RoomTitle40 = RoomTitle40 of string

type Room = {
  Id: RoomGuid
  Title: RoomTitle40
  Paper: Paper
  Students: Map<User.StudentGuid, User.Student>
  Hosts: Map<User.HostGuid, User.Host>
}

module Room =
  let init title = {
    Id = RoomGuid.NewGuid ()
    Title = title
    Paper = []
    Students = Map.empty
    Hosts = Map.empty
  }

module Name20 =
  let create name =
    if name |> String.length > 20 then None
    else Some (Name20 name)

module RoomTitle40 =
  let create title =
    if title |> String.length > 40 then None
    else Some (RoomTitle40 title)