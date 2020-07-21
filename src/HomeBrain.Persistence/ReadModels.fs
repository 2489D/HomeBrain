module HomeBrain.ReadModels

open System
open Domain
open Domain.Message

type StudentPaperModel = {
  ExamPaper: Paper
}

type StudentMessageModel = {
  Notices: Notice list
  Messages: MessageFromHostToStudent list
}

type HostMessageModel = {
  MessagesAmongHosts: MessageFromHostToHost list
  MessagesFromStudents: MessageFromStudentToHost list
}

type StudentModel = {
  Paper: StudentPaperModel
  Messages: StudentMessageModel
}

type HostModel = {
  Messages: HostMessageModel
}

/// Student Query
///   1. open paper
///   2. read a message
/// Host Query
///   1. read a message
/// Student Action
///   1. send a message
///   2. enter room
///   3. exit room
/// Host Action
///   0. edit a paper
///   1. send a message
///   2. enter room
///   3. exit room
///   4. start a exam
///   5. end a exam