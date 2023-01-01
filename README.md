# CatEars EasyConstruct

## Elevator Pitch

1. You want to create test objects as simply and maintainable as possible
2. How you construct your objects and test data always changes
3. You are annoyed that when you update the constructor you need to change 50 tests 
   and wish there was a library that made this easy for you.

## Ideas:

- Maybe rebrand as HappyBuild
- Add support for dynamic building where types are registered just-in-time
- Allow specifying values for specific constructor parameters
- Create a single static function `AutoResolve()` that is dynamic and "just builds it".
- Add support for classes with type parameters
- 