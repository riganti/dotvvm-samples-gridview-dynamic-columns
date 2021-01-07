# Generating GridView columns dynamically in DotVVM

This sample shows how to build dynamic columns in DotVVM. Please note that this is quite an advanced technique. 

If you are thinking about scaffolding data-grids automatically, check out our [DotVVM Dynamic Data](https://github.com/riganti/dotvvm) project - it may have everything you need.
 
## Creating GridView columns in the runtime

There are a few steps that you need to do:

1. First, Find the `GridView` control by its ID. Make sure you add `ID="GridView"` to your control in the markup.

```var gridView = (DotVVM.BusinessPack.Controls.GridView)Context.View.FindControlByClientId("GridView");```

2. Now, we'll need to build the binding expression itself. In order to do this, you will first need to obtain the "data context stack" - it represents the hierarchy of DataContext changes in the page from the root to the current DataContext, so the binding expression would know the types of `_this`, `_parent`, `_root` and so on. The easiest way to obtain it is to call `GetDataContextType` on the target control where you will be using the binding.

```
var dataContextStack = gridView.Columns[0].GetDataContextType();
```

3. Now we'll need to compose the LINQ expression representing the binding. Let's say we want to build something like this: `{value: ProductName}`. 
The LINQ expression needs to obtain the hierarchy of dataContexts - it gets them as an array of objects where first one (zero index) is `_this`, second one is `_parent` and the last one is `_root`. Of course, if the binding will be directly in the root viewmodel, the array will have only one item. 

The expression we are building will therefore look like this: `(object[] dataContexts) => ((Product)dataContexts[0]).ProductName`

```
var param = Expression.Parameter(typeof(object[]));
var expression = Expression.Lambda<Func<object[], TType>>(
    Expression.Property(
        Expression.Convert(
            Expression.ArrayIndex(param, Expression.Constant(0)), typeof(Product)
        ),
        "ProductName")
    , param);
```

It's easier to read it from the middle:
* `Expression.ArrayIndex(param, Expression.Constant(0))` is `dataContexts[0]`
* `Expression.Convert` is the cast to `Product`
* `Expression.Property` accesses the `ProductName` property
* And the final `Expression.Lambda` binds the body and parameters (in this case there is only one) together

4. Finally, you need to build the value binding expression. To do that, you will need `BindingCompilationService` - the easiest way to obtain it is to use dependency injection - just add a parameter of type `BindingCompilationService` in the viewmodel constructor.

```
var binding = ValueBindingExpression.CreateBinding(bindingCompilationService, expression, dataContextStack);
```

5. Now you have the value binding that you can use in your column:

```
// create the column
var gridViewTextColumn = new DotVVM.BusinessPack.Controls.GridViewTextColumn(bindingCompilationService)
{
    HeaderText = column.Name,
    ColumnName = column.PropertyName,
    SortExpression = column.PropertyName
};
gridViewTextColumn.SetBinding(DotVVM.BusinessPack.Controls.GridViewTextColumn.ValueProperty, binding);
gridView.Columns.Add(gridViewTextColumn);
```

