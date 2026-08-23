# Desarrollo de Aplicaciones Móviles II
Repositorio para subir las actividad de la materia

## Actividad 2 - Unidad I (Patrón MVVM)
### Objetivo: Una app que muestre un perfil editable del alumno
### Requisitos técnicos
1) Uso de MVVM completo (Model, ViewModel y View separados): UserProfile.cs (Carpeta Models), ProfileViewModel.cs (Carpeta ViewModels) y MainPage.xaml (Carpeta Views). Cada archivo con sus responsabilidades
2) Implementación de INotifyPropertyChanged: Se utilizar en ProfileViewModel para notificar a la vista cada vez que el valor cambia.
3) Uso de Command para el botón de guardar: El boton de guardar utiliza un SaveCommand que ejecuta de manera asincrona el método ExecuteSave
4) Binding bidireccional (TwoWay) para los campos editables: En el archivo MainPage.xaml en cada Entry y Editor tiene explicitamente configurado Text="{Binding Propiedad, Mode=TwoWay}". Esto garantiza que los datos vayan de la vista al ViewModel y viceversa. 
5) Validación básica (por ejemplo, que el nombre no esté vacío): Se agrego controles en los campos de "nombre completo" y "edad". El en nombre completo se valida que no sea nulo el string y en la edad ademas de verificar si no es nulo tambien que no sea menor o igual a 0(cero). Cada campo cuenta con un mensaje de error que se muestra cuando la validación detecta un valor incorrecto. En el caso del campo Edad, inicialmente se definió como un tipo string, ya que al configurarlo como int era posible enviar el campo vacío sin que se generara un error. Por este motivo, se implementó una transformación al momento de completar el campo para que solo acepte valores numéricos y permita validar correctamente los valores nulos o vacíos.
6) Diseño responsive usando StackLayout, Grid o VerticalStackLayout: Se envolvio toda la pantalla en un ScrollView para que se adapte a cualquier pantalla y se organizaron los elementos con VerticalStackLayout con espacios para que se vea correctamente cuando el usuario lo visualice. 
