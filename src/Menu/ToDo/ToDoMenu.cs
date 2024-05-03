namespace Paraclete.Menu.ToDo;

public class ToDoMenu(IServiceProvider services)
    : MenuBase(services, [
        typeof(AddToDoItemCommand),
        typeof(EditToDoItemCommand),
        typeof(EditToDoDateCommand),
        typeof(DeleteSelectedToDoItemCommand),
        typeof(SortToDoItemsCommand),
        typeof(ToggleItemDoneCommand),
        typeof(ToggleItemMoveModeCommand),
        typeof(PreviousItemCommand),
        typeof(NextItemCommand),
    ])
{
}
