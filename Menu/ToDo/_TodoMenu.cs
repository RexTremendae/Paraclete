namespace Paraclete.Menu.ToDo;

public class ToDoMenu : MenuBase
{
    public ToDoMenu(IServiceProvider services)
        : base(services, new Type[]
    {
        typeof(AddToDoItemCommand),
        typeof(EditToDoItemCommand),
        typeof(EditToDoDateCommand),
        typeof(DeleteSelectedToDoItemCommand),
        typeof(SortToDoItemCommand),
        typeof(ToggleItemDoneCommand),
        typeof(ToggleItemMoveModeCommand),
        typeof(PreviousItemCommand),
        typeof(NextItemCommand),
    })
    {
    }
}
