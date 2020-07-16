module HomeBrain.Domain
open System

type Name20 = Name20 of string

type File =
  | Photo
  | PDF
  | Essay

[<NoEquality; NoComparison>]
type HostData = {
  Name: Name20
}

type StudentId = IdString of string

type StudentData = {
  Name: Name20
  StdId: StudentId
}

[<NoEquality; NoComparison>]
type Submission = {
  Files: File list
  Date: DateTime
}

type String40 = String40 of string

type Student = StudentData * Submission list
type Host = HostData

// Shares server time
type Room = {
  Id: Guid
  Title: String40
  Hosts: Student list
  Students: Host list
}

type String300 = String300 of string

type User =
  | Student of Student
  | Host of Host

type RequestFn = unit -> unit // FIXME

type MessageContent =
  | String300
  | RequestFn

type MessageFromStudentToHost = {
  Sender: Student
  Receivers: Host list
  Content: MessageContent
}

type MessageFromHostToStudent = {
  Sender: Host
  Receivers: Student list
  Content: MessageContent
}

type MessageFromHostToHost = {
  Sender: Host
  Receivers: Host list
  Content: MessageContent
}

type Notice = {
  Sender: Host
  Receiver: User list
  Content: MessageContent
}

type Message =
  | MessageFromStudentToHost of MessageFromStudentToHost
  | MessageFromHostToStudent of MessageFromHostToStudent
  | MessageFromHostToHost of MessageFromHostToHost
  | Notice of Notice
