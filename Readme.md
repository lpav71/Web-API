# Реализация API интерфейса на C# NET CORE

При первом запуске будет автоматически создана база данных и заполнена тестовыми данными

## Описание API интерфейса

1. Получение списка сотрудников
   Формат запроса: GET api/stuffs/page/{id}
   Пример: api/stuffs/page/1
   {id} - номер страницы
   Пример ответа:
   ```json
   {
       "realtor": [
           {
               "id": 1,
               "firstName": "Иван",
               "lastName": "Иванов",
               "createdDateTime": "2021-04-11T12:43:43.2074524",
               "divisionId": 1,
               "division": {
                   "id": 1,
                   "name": "Краснодар",
                   "createdDateTime": "2021-04-11T12:43:43.3245923"
               }
           },
           {
               "id": 2,
               "firstName": "Яна",
               "lastName": "Константинова",
               "createdDateTime": "2021-04-11T12:43:43.323317",
               "divisionId": 1,
               "division": {
                   "id": 1,
                   "name": "Краснодар",
                   "createdDateTime": "2021-04-11T12:43:43.3245923"
               }
           }
       ],
       "pageViewModel": {
           "pageNumber": 1,
           "totalPages": 3
       }
   }
   ```

2. Получение данных по конкретному сотруднику
   Формат запроса: GET api/stuffs/2
   Пример: api/stuffs/2
   {id} - id сотрудника
   Пример ответа:
   ```json
   {
       "id": 2,
       "firstName": "Яна",
       "lastName": "Константинова",
       "createdDateTime": "2021-04-11T12:43:43.323317",
       "divisionId": 1,
       "division": null
   }
   ```

3. Получение данных по части фамилии сотрудника

   Формат запроса: GET api/stuffs/lastname/{LastName}/page/{id}
   Пример: api/stuffs/lastname/ив/page/1
   {id} - id сотрудника
   {LastName} - Часть имени сотрудника
   Пример ответа:
   ```json
   {
       "realtor": [
           {
               "id": 1,
               "firstName": "Иван",
               "lastName": "Иванов",
               "createdDateTime": "2021-04-11T12:43:43.2074524",
               "divisionId": 1,
               "division": null
           }
       ],
       "pageViewModel": {
           "pageNumber": 1,
           "totalPages": 1
       }
   }
   ```

4. Получение данных по всем сотрудникам филиала
   Формат запроса: GET /api/stuffs/division/{division}
   Пример: /api/stuffs/division/Краснодар
   {division} - Подразделение
   Пример ответа:
   ```json
   [
       {
           "firstName": "Иван",
           "lastName": "Иванов",
           "createdDateTime": "2021-04-11T12:43:43.2074524",
           "name": "Краснодар"
       },
       {
           "firstName": "Яна",
           "lastName": "Константинова",
           "createdDateTime": "2021-04-11T12:43:43.323317",
           "name": "Краснодар"
       },
       {
           "firstName": "Сергей",
           "lastName": "Саранкин",
           "createdDateTime": "2021-04-11T14:08:34.4285467",
           "name": "Краснодар"
       }
   ]
   ```

5. Добавление сотрудника
   Формат запроса: POST api/stuffs
   Пример: /api/stuffs/
   Содержание тела запроса:
   Формат JSON
   Поля: LastName - фамилия, FirstName - имя, DivisionId - id подразделения
   Пример запроса:
   ```json
    {"FirstName": "Сергей", "LastName": "Варкин", "DivisionId": 1}
   ```
   Пример ответа:
   ```json
   {
       "id": 7,
       "firstName": "Сергей",
       "lastName": "Варкин",
       "createdDateTime": "2021-04-11T18:16:38.0435887+03:00",
       "divisionId": 1,
       "division": null
   }
   ```

6. Изменение информации о сотруднике (Требуются все данные)
   Формат запроса: PUT api/stuffs
   Пример: /api/stuffs/
   {id} - id сотрудника
   Содержание тела запроса:
   Формат JSON
   Поля: Id, LastName - фамилия, FirstName - имя, DivisionId - id подразделения
   (Все поля обязательны)
   Пример запроса:
   
   ```json
    {"Id": 5, "FirstName": "Сергей", "LastName": "Варкин", "DivisionId": 1}
   ```
   Пример ответа:
   ```json
   {
       "id": 6,
       "firstName": "Сергей",
       "lastName": "Саранкин",
       "createdDateTime": "2021-04-11T14:08:34.4285467",
       "divisionId": 1,
       "division": null
   }
   ```

7. Изменение информации о сотруднике (Данные требуются только частично)
   Формат запроса: PATCH api/stuffs/{id}
   Пример: /api/stuffs/
   {id} - id сотрудника
   Содержание тела запроса:
   Формат JSON
   Поля: LastName - фамилия, FirstName - имя, DivisionId - id подразделения
   (Все поля необязательны)
   Пример запроса:
   ```json
    {"FirstName": "Сергей", "LastName": "Варкин", "DivisionId": 1}
   ```

   Пример ответа:
   ```json
   {
       "id": 6,
       "firstName": "Сергей",
       "lastName": "Саранкин",
       "createdDateTime": "2021-04-11T14:08:34.4285467",
       "divisionId": 1,
       "division": null
   }
   ```

8. Удаление информации о сотруднике
   Формат запроса: DELETE api/stuffs/{id}
   Пример: /api/stuffs/
   {id} - id сотрудника
   Пример ответа:
   ```json
   {
       "id": 6,
    "firstName": "Сергей",
       "lastName": "Саранкин",
       "createdDateTime": "2021-04-11T14:08:34.4285467",
       "divisionId": 1,
       "division": null
   }
   ```
9. Получение списка подразделений
   Формат запроса: GET api/divisions/page/{id}
   Пример: api/divisions/page/1
   {id} - номер страницы
   Пример ответа:   
   ```json
   {
       "division": [
           {
               "id": 1,
               "name": "Краснодар",
               "createdDateTime": "2021-04-11T12:43:43.3245923"
           },
           {
               "id": 2,
               "name": "Москва",
               "createdDateTime": "2021-04-11T12:43:43.3363699"
           }
       ],
       "pageViewModel": {
           "pageNumber": 1,
           "totalPages": 2
       }
   }
   ```
   
10. Получить данные по конкретному подразделению
      Формат запроса: /api/divisions/{id}
      Пример запроса: /api/divisions/1
      Пример ответа:
   ```json
   {
       "id": 1,
       "name": "Краснодар",
       "createdDateTime": "2021-04-11T12:43:43.3245923"
   }
   ```

11. Добавление подразделения
      Формат запроса: POST api/divisions
      Пример: /api/divisions
      Содержание тела запроса:
      Формат JSON
      Поля: Name - фамилия
      Пример запроса:
   ```json
   {"Name": "Волгоград"}
   ```
   Пример ответа:
   ```json
   {
    "id": 5,
    "name": "Волгоград",
    "createdDateTime": "2021-04-11T18:38:08.9215821+03:00"
   }
   ```

12. Изменение подразделения
      Формат запроса: PUT api/divisions/{id}
      Пример: /api/divisions/
      Содержание тела запроса:
      Формат JSON
      Поля: Name - Название подразделения
      Пример запроса:
  ```json
   {"Name": "Курган"}
  ```
   пример ответа:
   ```json
   {
      "id": 5,
      "name": "Курган",
      "createdDateTime": "2021-04-11T18:38:08.9215821"
   }
   ```

13. Удаление подразделения
    формат запроса: DELETE api/divisions/{id}
    Пример: api/divisions/5
    Пример ответа:    
    ```json
    {
    "id": 5,
        "name": "Курган",
        "createdDateTime": "2021-04-11T18:38:08.9215821"
    }
    ```
    
