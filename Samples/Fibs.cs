 using  VplLibrary  ;  public class  Fibs  {  public  dynamic  Старт  (  ){  dynamic  var0  ;  var0  =  Фибоначчи  (  4  )  ;  Вывод.Сообщение  (  var0  )  ;  }  public  dynamic  Фибоначчи  (  dynamic  n  ){  dynamic  a  ;  if(  (  n  <  3  )  ){  return  1  ;  }  a  =  Фибоначчи  (  (  n  -  1  )  )  ;  return  (  a  +  Фибоначчи  (  (  n  -  2  )  )  )  ;  }  } 