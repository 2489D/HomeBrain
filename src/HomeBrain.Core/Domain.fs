module HomeBrain.Domain
open System

type Name20 = Name20 of string

type Upload =
  | Photo
  | PDF

/// TODO
/// 1. How to represent file in F#
/// 2. How to represent html forms or relevant data in this type `File`
type File =
  | Upload
  | Form 

[<NoEquality; NoComparison>]
type Submission = {
  Files: File list
  Date: DateTimeOffset
}

module User =
  type StudentId = IdString of string

  [<NoEquality; NoComparison>]
  type HostData = {
    Name: Name20
  }

  [<NoEquality; NoComparison>]
  type StudentData = {
    Name: Name20
    StdId: StudentId
  }

  type Student = StudentData * Submission list
  type Host = HostData

  type User =
    | Student of Student
    | Host of Host


type String40 = String40 of string

// Shares server time

module Message =
  type MsgString = MsgString of string
  type RequestFn = unit -> unit // FIXME

  type MessageContent =
    | MsgString of MsgString
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


type Room = {
  Id: Guid
  Title: String40
  Hosts: User.Host list
  Students: User.Student list
}
