﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace КурсовойПроект.Service 
{
    //если для какого-либо контроллера в области админ встречается атрибут area, то в этом случае подключаем авторизацию
    //и аутентификацию, и в startupcs определили правило, что пользователь должен состоять в роли admin
    //все неавторизованные пользователи направляются по пути /account/login, где они направляются для ввода своих данных. Если он их ввёл правильно, он отправляется к панели администратора
    public class AdminAreaAuthorization : IControllerModelConvention
    {
        private readonly string area;
        private readonly string policy;

        //area должна подпадать под политику

        public AdminAreaAuthorization(string area, string policy)
        {
            this.area = area;
            this.policy = policy;
        }
        //проверка для контроллера его атрибуты (если присутствует атрибут Area, то добавляем фильтр для контроллера AuthorizeFilter, то есть отправляем пользователя на авторизацию)
        public void Apply(ControllerModel controller)
        {
            if (controller.Attributes.Any(a =>
                    a is AreaAttribute && (a as AreaAttribute).RouteValue.Equals(area, StringComparison.OrdinalIgnoreCase))
                || controller.RouteValues.Any(r =>
                    r.Key.Equals("area", StringComparison.OrdinalIgnoreCase) && r.Value.Equals(area, StringComparison.OrdinalIgnoreCase)))
            {
                controller.Filters.Add(new AuthorizeFilter(policy));
            }
        }
    }
}
