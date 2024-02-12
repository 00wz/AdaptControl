`Павел Тесленко`
***

#### Прототип стратегии в стиле Black & White

[GameRoot](https://github.com/00wz/AdaptControl/blob/main/Assets/Scripts/GameRoot.cs) - монобех. имеет сериализованные поля для всех важных классов и объектов на сцене. доступ к нему можно получить через статическое поле. имеет ссылки на:
* [CameraMove](https://github.com/00wz/AdaptControl/blob/main/Assets/Scripts/CameraMove.cs) - класс отвечающий за передвижение камеры как в стратегиях, и указание целей.
* [DialogView](https://github.com/00wz/AdaptControl/blob/main/Assets/Scripts/UI/DialogView.cs) - класс отвечающий за отображение диалогов и сообщений.
* [BuildSystem](https://github.com/00wz/AdaptControl/blob/main/Assets/Scripts/BuildSystem/BuildSystem.cs) - класс отвечающий за строительство.

[GodsHand](https://github.com/00wz/AdaptControl/blob/main/Assets/Scripts/GodsHand.cs) - перемещение персонажей по воздуху с помощью мыши.

[Character](https://github.com/00wz/AdaptControl/blob/main/Assets/Scripts/Characters/Character.cs) - надстройка над navMeshAgent. его наследник - [Worker](https://github.com/00wz/AdaptControl/blob/main/Assets/Scripts/Characters/Worker/Worker.cs) - персонаж способный работать. имеет [стейтмашину](https://github.com/00wz/AdaptControl/tree/main/Assets/Scripts/StateMachine) с состояниями [поиска работы](https://github.com/00wz/AdaptControl/blob/main/Assets/Scripts/Characters/Worker/States/WorkerSearchState.cs), [работы](https://github.com/00wz/AdaptControl/blob/main/Assets/Scripts/Characters/Worker/States/WorkerWorkState.cs), и [перемещения к рабочему месту](https://github.com/00wz/AdaptControl/blob/main/Assets/Scripts/Characters/Worker/States/WorkerGoingToWorkplaceState.cs).

[StorySystem](https://github.com/00wz/AdaptControl/blob/main/Assets/Scripts/StorySystem/StorySystem.cs) - позволяет создать линейную цепочку событий из классов реализующих _IStoryPoint_ (событие после которого сразу выполнянтся следующее) и _IStoryLine_ (событие передает вызывает следующее только после своего завершения). для кастомизации StorySystem в инспекторе использовал [Serializable Interface](https://github.com/Thundernerd/Unity3D-SerializableInterface.git)

