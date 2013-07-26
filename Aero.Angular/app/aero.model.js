/* model: extend server-supplied metadata with client-side entity model members */
aero.factory('model', ['$rootScope', function ($rootScope) {

    var datacontext;
    
    extendTodoList();
    
    var model = {
        initialize: initialize,
        rfqInitializer: rfqInitializer
    };
    
    return model;
  
    //#region private members
    function initialize(context) {
        datacontext = context;
        var store = datacontext.metadataStore;
        //store.registerEntityTypeCtor("RFQ", null, rfqInitializer);
        //store.registerEntityTypeCtor("TodoItem", null, todoItemInitializer);
        //store.registerEntityTypeCtor("TodoList", TodoList, todoListInitializer);
    }
    
    function todoItemInitializer(todoItem) {
        todoItem.errorMessage = "";
    }

    function rfqInitializer(rfq) {
        rfq.id = -1;
        var d = new Date();
        d.setDate(d.getDate() + 7);
        d.setHours(0, 0, 0, 0);
        rfq.needBy = d;
        rfq.qty = 1;
        rfq.priorityId = 1;
        rfq.rFQStateId = 1;
    }

    function todoListInitializer(todoList) {
        todoList.errorMessage = "";
        todoList.newTodoTitle = "";
        todoList.isEditingListTitle = false;
    }

    function TodoList() {
        this.title = "My todos"; // defaults
        this.userId = "to be replaced";
    }

    function extendTodoList() {
        TodoList.prototype.addTodo = function () {
            var todoList = this;
            var title = todoList.newTodoTitle;
            if (title) { // need a title to save
                var todoItem = datacontext.createTodoItem();
                todoItem.title = title;
                todoItem.todoList = todoList;
                datacontext.saveEntity(todoItem);
                todoList.newTodoTitle = ""; // clear UI title box
            }
        };

        TodoList.prototype.deleteTodo = function (todo) {
            return datacontext.deleteTodoItem(todo);
        };
    }
    //#endregion
}]);