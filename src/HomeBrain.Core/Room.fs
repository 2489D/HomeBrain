module HomeBrain.Room
open HomeBrain.Domain

type ExamState =
  | IsWaiting
  | OnExam
  | ExamFinished
  | IsClosed

let initialExamState = IsWaiting

type RoomState = {
  title: String40
  hosts: User array
  students: User array
  examState: ExamState
}

let initialRoomState = {
  title = ""
  hosts = []
  students = []
  examState = initialExamState
}

let startExam state =
  match state.examState with
  | IsWaiting -> Ok { state with examState = OnExam }
  | OnExam -> Error ExamAlreadyStarted
  | ExamFinished -> Error ExamAlreadyEnded
  | IsClosed -> Error NotValidRoom

let endExam state =
  | IsWaiting -> Error ExamNotStarted
  | OnExam -> Ok { state with examState = ExamFinished }
  | ExamFinished -> Error ExamAlreadyEnded
  | IsClosed -> Error NotValidRoom

let closeRoom state =
  | IsWaiting -> Ok { state with examState = IsClosed }
  | OnExam -> Error CannotCloseRoomDuringExam
  | ExamFinished -> Ok { state with examState = IsClosed }
  | IsClosed -> Error NotValidRoom

let enterRoom user state =
  match user with
  | Student s -> Ok { state with students = Array.append [| Student s |] state.students }
  | Host h -> Ok { state with hosts = Array.append [| Host h |] state.hosts }

let exitRoom user state =
  match user with
  | Student s -> Ok { state with students = Array.filter ((<>) (Student s)) state.students }
  | Host h -> Ok { state with hosts = Array.filter ((<>) (Host h)) state.students }