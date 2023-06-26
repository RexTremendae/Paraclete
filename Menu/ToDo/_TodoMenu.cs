namespace Paraclete.Menu.ToDo;

public class _ToDoMenu : MenuBase
{
    public _ToDoMenu(IServiceProvider services)
        : base(services, new Type[]
    {
        typeof(AddToDoItemCommand),
        typeof(EditToDoItemCommand),
        typeof(EditToDoDateCommand),
        typeof(DeleteSelectedToDoItemCommand),
        typeof(ToggleItemDoneCommand),
        typeof(ToggleItemMoveModeCommand),
        typeof(PreviousItemCommand),
        typeof(NextItemCommand)
    })
    {}
}
