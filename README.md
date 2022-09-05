# Курс "ASP.NET Core Web API микросервисы"
## Урок 1. Повторение ООП. Первый микросервис.
Написать свой контроллер и методы в нем, которые бы предоставляли следующую функциональность:
- Возможность сохранить температуру в указанное время
- Возможность отредактировать показатель температуры в указанное время
- Возможность удалить показатель температуры в указанный промежуток времени
- Возможность прочитать список показателей температуры за указанный промежуток времени

## Урок 2. REST API и тесты.
1. Добавьте метод в контроллер агентов проекта, относящегося к менеджеру метрик, который
позволяет получить список зарегистрированных в системе объектов.
2. В проект агента сбора метрик добавьте контроллеры для сбора метрик, аналогичные
менеджеру сбора метрик. Добавьте методы для получения метрик с агента, доступные по
следующим путям:
    - api/metrics/cpu/from/{fromTime}/to/{toTime}.
    - api/metrics/dotnet/errors-count/from/{fromTime}/to/{toTime}.
    - api/metrics/network/from/{fromTime}/to/{toTime}.
    - api/metrics/hdd/left/from/{fromTime}/to/{toTime}.
    - api/metrics/ram/available/from/{fromTime}/to/{toTime}.
3. Добавьте проект с тестами для агента сбора метрик. Напишите простые Unit-тесты на каждый
метод отдельно взятого контроллера в обоих тестовых проектах.

## Урок 3. Работа с базами данных.
1. Добавьте логирование всех параметров в каждом из контроллеров в обоих проектах
2. Добавьте репозитории для каждого типа метрик в сервис агент сбора метрик
3. Добавьте обработчики в контроллеры в REST стиле для наполнения метриками базы данных
4. Добавьте тесты на все контроллеры и все методы с использованием заглушек