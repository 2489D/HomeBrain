module HomeBrain.Projections

open Events
open Domain
open Domain.Message

type StudentActions = {
  ReadMessages: Message -> Async<unit>
}

type HostActions = {
}

type ProjectionActions = {
  Student: StudentActions
  Host: HostActions
}

let projectReadModel actions = function
| MessageSent (id, msg) ->
  [actions.Student.ReadMessages msg] |> Async.Parallel
| _ -> //TODO