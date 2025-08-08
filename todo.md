## MUST

### General
- [ ] unify navigation settings
- [ ] remove testing page

### DB Service
- [ ] re-load db service since entry point changed

### User Settings
- [x] connect to user pfp
- [ ] merge preferences with user settings
- [ ] persist user settings

### ToDo List
- [x] connect todo list
- [ ] save / load tasks
- [ ] allow adding and deleting UserTasks

### Calendar
- [ ] make calendar
- [ ] connect calendar (prepped)
- [ ] connect to TaskDetailPage

## SHOULD

- [x] fix theming to be light mode for readability
	- remove black background
	- revert text color to black
- [ ] cleanup navigation stack
	- editing then saving notes shouldn't add to stack
	- clicking nav bar repeatedly stacks same location
	- duplicate back buttons (image and top nav)

## COULD

### General
- [ ] redo custom theming
- [ ] unify design
	- headings and page titles are inconsistent

### To-do list
- [ ] truncate UserTask description in list

## EVENTUALLY
- [ ] speed up CI