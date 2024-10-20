using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicFunction.Helper
{

    public interface IErrorHandler
    {

        Dictionary<ErrorHandler.Language, List<ErrorHandler.Error>> GetErrors();
        bool CleanErrors();
        bool AddError(ErrorHandler.Language language, List<ErrorHandler.Error> errors);
        public string GetErrorMessage(ErrorHandler.Language language, Exception ex);
    }

    public class ErrorHandler : IErrorHandler
    {
        public class Error
        {
            public string Message { get; set; }
            public string TranslationOfTheMessage { get; set; }
        }

        public enum Language
        {
            en, // English
            fr, // French
            es, // Spanish
            de, // German
            it, // Italian
            ru, // Russian
            zh, // Chinese
            ja, // Japanese
            ko, // Korean
            pt, // Portuguese
            ar, // Arabic
            hi, // Hindi
            pl, // Polish   ********
            tr, // Turkish
            nl, // Dutch
            sv, // Swedish
            da, // Danish
            fi, // Finnish
            no, // Norwegian
            cs, // Czech
            hu, // Hungarian
            ro, // Romanian
            sk, // Slovak
            bg, // Bulgarian
            th, // Thai
            vi, // Vietnamese
            ms, // Malay    ******
            id, // Indonesian
            sw, // Swahili
            he, // Hebrew
            et, // Estonian
            lv, // Latvian
            lt, // Lithuanian
            sl, // Slovenian
            ca, // Catalan
            gl, // Galician
            fa, // Persian
            tt, // Tatar
            uk, // Ukrainian
            ml, // Malayalam
            ta, // Tamil
            te, // Telugu
            kn, // Kannada
            bn, // Bengali
            am, // Amharic
            ne  // Nepali
        }

        private Dictionary<Language, List<Error>> errorTranslations;

        public Dictionary<Language, List<Error>> GetErrors()
        {
            return errorTranslations;
        }

        public bool CleanErrors()
        {
            try
            {
                errorTranslations = new Dictionary<Language, List<Error>>();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool AddError(Language language, List<Error> errors)
        {
            // Check if the errorTranslations dictionary has an entry for the given language
            if (!errorTranslations.ContainsKey(language))
            {
                errorTranslations[language] = new List<Error>(); // Initialize if not present
            }

            // Check if any of the incoming errors are already in the errorTranslations
            foreach (var error in errors)
            {
                // Check if the message already exists in the errorTranslations for the specified language
                if (!errorTranslations[language].Any(e => e.Message == error.Message))
                {
                    errorTranslations[language].Add(error); // Add new error
                }
                else
                {
                    return false; // Return false if any error already exists
                }
            }

            return true; // Return true if all errors were added successfully
        }

        public ErrorHandler()
        {
            errorTranslations = new Dictionary<Language, List<Error>>
            {
                { Language.en, new List<Error>
                    {
                        new Error { Message = "NullReferenceException", TranslationOfTheMessage = "Object reference not set to an instance of an object." },
                        new Error { Message = "ArgumentException", TranslationOfTheMessage = "Invalid argument provided." },
                        new Error { Message = "InvalidOperationException", TranslationOfTheMessage = "Operation is not valid due to the current state." },
                        new Error { Message = "IndexOutOfRangeException", TranslationOfTheMessage = "Index was outside the bounds of the array." },
                        new Error { Message = "DivideByZeroException", TranslationOfTheMessage = "Attempted to divide by zero." },
                        new Error { Message = "FormatException", TranslationOfTheMessage = "Input string was not in a correct format." },
                        new Error { Message = "InvalidCastException", TranslationOfTheMessage = "Specified cast is not valid." },
                        new Error { Message = "NotImplementedException", TranslationOfTheMessage = "The method or operation is not implemented." },
                        new Error { Message = "TimeoutException", TranslationOfTheMessage = "The operation has timed out." },
                        new Error { Message = "FileNotFoundException", TranslationOfTheMessage = "The specified file was not found." },
                        new Error { Message = "UnauthorizedAccessException", TranslationOfTheMessage = "Access to the path is denied." },
                        new Error { Message = "KeyNotFoundException", TranslationOfTheMessage = "The given key was not present in the dictionary." },
                        new Error { Message = "OutOfMemoryException", TranslationOfTheMessage = "Not enough memory to continue the execution of the program." },
                        new Error { Message = "StackOverflowException", TranslationOfTheMessage = "The requested operation caused a stack overflow." },
                        new Error { Message = "AccessViolationException", TranslationOfTheMessage = "Attempted to read or write protected memory." },
                        new Error { Message = "SqlException", TranslationOfTheMessage = "A SQL Server error occurred." },
                        new Error { Message = "IOException", TranslationOfTheMessage = "An I/O error occurred." },
                        new Error { Message = "OperationCanceledException", TranslationOfTheMessage = "The operation was canceled." },
                        new Error { Message = "ArgumentNullException", TranslationOfTheMessage = "Value cannot be null." },
                        new Error { Message = "NotSupportedException", TranslationOfTheMessage = "The operation is not supported." },
                        new Error { Message = "ObjectDisposedException", TranslationOfTheMessage = "Cannot access a disposed object." },
                        new Error { Message = "ArithmeticException", TranslationOfTheMessage = "An error occurred during arithmetic operations." },
                        new Error { Message = "DirectoryNotFoundException", TranslationOfTheMessage = "The specified directory does not exist." },
                        new Error { Message = "PathTooLongException", TranslationOfTheMessage = "The specified path or filename is too long." }
                    }
                },
                { Language.fr, new List<Error>
                    {
                        new Error { Message = "NullReferenceException", TranslationOfTheMessage = "Référence d'objet non définie à une instance d'un objet." },
                        new Error { Message = "ArgumentException", TranslationOfTheMessage = "Argument invalide fourni." },
                        new Error { Message = "InvalidOperationException", TranslationOfTheMessage = "L'opération n'est pas valide en raison de l'état actuel." },
                        new Error { Message = "IndexOutOfRangeException", TranslationOfTheMessage = "L'index était en dehors des limites du tableau." },
                        new Error { Message = "DivideByZeroException", TranslationOfTheMessage = "Tentative de division par zéro." },
                        new Error { Message = "FormatException", TranslationOfTheMessage = "La chaîne d'entrée n'était pas dans un format correct." },
                        new Error { Message = "InvalidCastException", TranslationOfTheMessage = "Le cast spécifié n'est pas valide." },
                        new Error { Message = "NotImplementedException", TranslationOfTheMessage = "La méthode ou l'opération n'est pas implémentée." },
                        new Error { Message = "TimeoutException", TranslationOfTheMessage = "L'opération a expiré." },
                        new Error { Message = "FileNotFoundException", TranslationOfTheMessage = "Le fichier spécifié est introuvable." },
                        new Error { Message = "UnauthorizedAccessException", TranslationOfTheMessage = "L'accès au chemin est refusé." },
                        new Error { Message = "KeyNotFoundException", TranslationOfTheMessage = "La clé donnée n'était pas présente dans le dictionnaire." },
                        new Error { Message = "OutOfMemoryException", TranslationOfTheMessage = "Pas assez de mémoire pour continuer l'exécution du programme." },
                        new Error { Message = "StackOverflowException", TranslationOfTheMessage = "L'opération demandée a causé un débordement de pile." },
                        new Error { Message = "AccessViolationException", TranslationOfTheMessage = "Tentative de lire ou d'écrire dans une mémoire protégée." },
                        new Error { Message = "SqlException", TranslationOfTheMessage = "Une erreur de serveur SQL s'est produite." },
                        new Error { Message = "IOException", TranslationOfTheMessage = "Une erreur d'E/S s'est produite." },
                        new Error { Message = "OperationCanceledException", TranslationOfTheMessage = "L'opération a été annulée." },
                        new Error { Message = "ArgumentNullException", TranslationOfTheMessage = "La valeur ne peut pas être nulle." },
                        new Error { Message = "NotSupportedException", TranslationOfTheMessage = "L'opération n'est pas prise en charge." },
                        new Error { Message = "ObjectDisposedException", TranslationOfTheMessage = "Impossible d'accéder à un objet éliminé." },
                        new Error { Message = "ArithmeticException", TranslationOfTheMessage = "Une erreur est survenue lors des opérations arithmétiques." },
                        new Error { Message = "DirectoryNotFoundException", TranslationOfTheMessage = "Le répertoire spécifié n'existe pas." },
                        new Error { Message = "PathTooLongException", TranslationOfTheMessage = "Le chemin ou le nom de fichier spécifié est trop long." }
                    }
                },
                { Language.es, new List<Error>
                    {
                        new Error { Message = "NullReferenceException", TranslationOfTheMessage = "Referencia de objeto no establecida en una instancia de un objeto." },
                        new Error { Message = "ArgumentException", TranslationOfTheMessage = "Argumento inválido proporcionado." },
                        new Error { Message = "InvalidOperationException", TranslationOfTheMessage = "La operación no es válida debido al estado actual." },
                        new Error { Message = "IndexOutOfRangeException", TranslationOfTheMessage = "El índice estaba fuera de los límites del arreglo." },
                        new Error { Message = "DivideByZeroException", TranslationOfTheMessage = "Intento de división por cero." },
                        new Error { Message = "FormatException", TranslationOfTheMessage = "La cadena de entrada no estaba en un formato correcto." },
                        new Error { Message = "InvalidCastException", TranslationOfTheMessage = "La conversión especificada no es válida." },
                        new Error { Message = "NotImplementedException", TranslationOfTheMessage = "El método o la operación no están implementados." },
                        new Error { Message = "TimeoutException", TranslationOfTheMessage = "La operación ha excedido el tiempo de espera." },
                        new Error { Message = "FileNotFoundException", TranslationOfTheMessage = "El archivo especificado no fue encontrado." },
                        new Error { Message = "UnauthorizedAccessException", TranslationOfTheMessage = "Acceso al camino denegado." },
                        new Error { Message = "KeyNotFoundException", TranslationOfTheMessage = "La clave dada no estaba presente en el diccionario." },
                        new Error { Message = "OutOfMemoryException", TranslationOfTheMessage = "No hay suficiente memoria para continuar la ejecución del programa." },
                        new Error { Message = "StackOverflowException", TranslationOfTheMessage = "La operación solicitada causó un desbordamiento de pila." },
                        new Error { Message = "AccessViolationException", TranslationOfTheMessage = "Se intentó leer o escribir en una memoria protegida." },
                        new Error { Message = "SqlException", TranslationOfTheMessage = "Se produjo un error en el servidor SQL." },
                        new Error { Message = "IOException", TranslationOfTheMessage = "Se produjo un error de E/S." },
                        new Error { Message = "OperationCanceledException", TranslationOfTheMessage = "La operación fue cancelada." },
                        new Error { Message = "ArgumentNullException", TranslationOfTheMessage = "El valor no puede ser nulo." },
                        new Error { Message = "NotSupportedException", TranslationOfTheMessage = "La operación no es compatible." },
                        new Error { Message = "ObjectDisposedException", TranslationOfTheMessage = "No se puede acceder a un objeto eliminado." },
                        new Error { Message = "ArithmeticException", TranslationOfTheMessage = "Ocurrió un error durante las operaciones aritméticas." },
                        new Error { Message = "DirectoryNotFoundException", TranslationOfTheMessage = "El directorio especificado no existe." },
                        new Error { Message = "PathTooLongException", TranslationOfTheMessage = "El camino o el nombre de archivo especificado es demasiado largo." }
                    }
                },
                { Language.de, new List<Error>
                    {
                        new Error { Message = "NullReferenceException", TranslationOfTheMessage = "Objektverweis wurde nicht auf eine Instanz eines Objekts festgelegt." },
                        new Error { Message = "ArgumentException", TranslationOfTheMessage = "Ungültiges Argument bereitgestellt." },
                        new Error { Message = "InvalidOperationException", TranslationOfTheMessage = "Die Operation ist aufgrund des aktuellen Status nicht gültig." },
                        new Error { Message = "IndexOutOfRangeException", TranslationOfTheMessage = "Index lag außerhalb der Grenzen des Arrays." },
                        new Error { Message = "DivideByZeroException", TranslationOfTheMessage = "Versuch, durch Null zu dividieren." },
                        new Error { Message = "FormatException", TranslationOfTheMessage = "Eingabezeichenfolge war nicht im richtigen Format." },
                        new Error { Message = "InvalidCastException", TranslationOfTheMessage = "Der angegebene Cast ist nicht gültig." },
                        new Error { Message = "NotImplementedException", TranslationOfTheMessage = "Die Methode oder Operation ist nicht implementiert." },
                        new Error { Message = "TimeoutException", TranslationOfTheMessage = "Die Operation hat das Zeitlimit überschritten." },
                        new Error { Message = "FileNotFoundException", TranslationOfTheMessage = "Die angegebene Datei wurde nicht gefunden." },
                        new Error { Message = "UnauthorizedAccessException", TranslationOfTheMessage = "Zugriff auf den Pfad verweigert." },
                        new Error { Message = "KeyNotFoundException", TranslationOfTheMessage = "Der angegebene Schlüssel war nicht im Wörterbuch vorhanden." },
                        new Error { Message = "OutOfMemoryException", TranslationOfTheMessage = "Nicht genügend Speicher, um die Ausführung des Programms fortzusetzen." },
                        new Error { Message = "StackOverflowException", TranslationOfTheMessage = "Die angeforderte Operation hat einen Stapelüberlauf verursacht." },
                        new Error { Message = "AccessViolationException", TranslationOfTheMessage = "Versuch, auf geschützten Speicher zu lesen oder zu schreiben." },
                        new Error { Message = "SqlException", TranslationOfTheMessage = "Es ist ein SQL-Serverfehler aufgetreten." },
                        new Error { Message = "IOException", TranslationOfTheMessage = "Es ist ein E/A-Fehler aufgetreten." },
                        new Error { Message = "OperationCanceledException", TranslationOfTheMessage = "Die Operation wurde abgebrochen." },
                        new Error { Message = "ArgumentNullException", TranslationOfTheMessage = "Wert kann nicht null sein." },
                        new Error { Message = "NotSupportedException", TranslationOfTheMessage = "Die Operation wird nicht unterstützt." },
                        new Error { Message = "ObjectDisposedException", TranslationOfTheMessage = "Kann nicht auf ein verworfenes Objekt zugreifen." },
                        new Error { Message = "ArithmeticException", TranslationOfTheMessage = "Ein Fehler ist während arithmetischer Operationen aufgetreten." },
                        new Error { Message = "DirectoryNotFoundException", TranslationOfTheMessage = "Das angegebene Verzeichnis existiert nicht." },
                        new Error { Message = "PathTooLongException", TranslationOfTheMessage = "Der angegebene Pfad oder Dateiname ist zu lang." }
                    }
                },
                { Language.it, new List<Error>
                    {
                        new Error { Message = "NullReferenceException", TranslationOfTheMessage = "Riferimento a oggetto non impostato su un'istanza di un oggetto." },
                        new Error { Message = "ArgumentException", TranslationOfTheMessage = "Argomento non valido fornito." },
                        new Error { Message = "InvalidOperationException", TranslationOfTheMessage = "Operazione non valida a causa dello stato attuale." },
                        new Error { Message = "IndexOutOfRangeException", TranslationOfTheMessage = "L'indice era al di fuori dei limiti dell'array." },
                        new Error { Message = "DivideByZeroException", TranslationOfTheMessage = "Tentativo di divisione per zero." },
                        new Error { Message = "FormatException", TranslationOfTheMessage = "La stringa di input non era nel formato corretto." },
                        new Error { Message = "InvalidCastException", TranslationOfTheMessage = "Il cast specificato non è valido." },
                        new Error { Message = "NotImplementedException", TranslationOfTheMessage = "Il metodo o l'operazione non sono implementati." },
                        new Error { Message = "TimeoutException", TranslationOfTheMessage = "L'operazione ha superato il timeout." },
                        new Error { Message = "FileNotFoundException", TranslationOfTheMessage = "Il file specificato non è stato trovato." },
                        new Error { Message = "UnauthorizedAccessException", TranslationOfTheMessage = "Accesso negato al percorso." },
                        new Error { Message = "KeyNotFoundException", TranslationOfTheMessage = "La chiave fornita non era presente nel dizionario." },
                        new Error { Message = "OutOfMemoryException", TranslationOfTheMessage = "Memoria insufficiente per continuare l'esecuzione del programma." },
                        new Error { Message = "StackOverflowException", TranslationOfTheMessage = "L'operazione richiesta ha causato un overflow dello stack." },
                        new Error { Message = "AccessViolationException", TranslationOfTheMessage = "Tentativo di leggere o scrivere in memoria protetta." },
                        new Error { Message = "SqlException", TranslationOfTheMessage = "Si è verificato un errore del server SQL." },
                        new Error { Message = "IOException", TranslationOfTheMessage = "Si è verificato un errore di I/O." },
                        new Error { Message = "OperationCanceledException", TranslationOfTheMessage = "L'operazione è stata annullata." },
                        new Error { Message = "ArgumentNullException", TranslationOfTheMessage = "Il valore non può essere nullo." },
                        new Error { Message = "NotSupportedException", TranslationOfTheMessage = "L'operazione non è supportata." },
                        new Error { Message = "ObjectDisposedException", TranslationOfTheMessage = "Impossibile accedere a un oggetto eliminato." },
                        new Error { Message = "ArithmeticException", TranslationOfTheMessage = "Si è verificato un errore durante le operazioni aritmetiche." },
                        new Error { Message = "DirectoryNotFoundException", TranslationOfTheMessage = "La directory specificata non esiste." },
                        new Error { Message = "PathTooLongException", TranslationOfTheMessage = "Il percorso o il nome del file specificato è troppo lungo." }
                    }
                },
                { Language.ru, new List<Error>
                    {
                        new Error { Message = "NullReferenceException", TranslationOfTheMessage = "Ссылка на объект не установлена в экземпляр объекта." },
                        new Error { Message = "ArgumentException", TranslationOfTheMessage = "Предоставлен недопустимый аргумент." },
                        new Error { Message = "InvalidOperationException", TranslationOfTheMessage = "Операция недействительна из-за текущего состояния." },
                        new Error { Message = "IndexOutOfRangeException", TranslationOfTheMessage = "Индекс был вне границ массива." },
                        new Error { Message = "DivideByZeroException", TranslationOfTheMessage = "Попытка деления на ноль." },
                        new Error { Message = "FormatException", TranslationOfTheMessage = "Входная строка не соответствует правильному формату." },
                        new Error { Message = "InvalidCastException", TranslationOfTheMessage = "Указанное приведение недопустимо." },
                        new Error { Message = "NotImplementedException", TranslationOfTheMessage = "Метод или операция не реализованы." },
                        new Error { Message = "TimeoutException", TranslationOfTheMessage = "Время операции истекло." },
                        new Error { Message = "FileNotFoundException", TranslationOfTheMessage = "Указанный файл не найден." },
                        new Error { Message = "UnauthorizedAccessException", TranslationOfTheMessage = "Доступ к пути запрещен." },
                        new Error { Message = "KeyNotFoundException", TranslationOfTheMessage = "Указанный ключ отсутствует в словаре." },
                        new Error { Message = "OutOfMemoryException", TranslationOfTheMessage = "Недостаточно памяти для продолжения выполнения программы." },
                        new Error { Message = "StackOverflowException", TranslationOfTheMessage = "Запрашиваемая операция вызвала переполнение стека." },
                        new Error { Message = "AccessViolationException", TranslationOfTheMessage = "Попытка чтения или записи в защищенную память." },
                        new Error { Message = "SqlException", TranslationOfTheMessage = "Произошла ошибка SQL Server." },
                        new Error { Message = "IOException", TranslationOfTheMessage = "Произошла ошибка ввода-вывода." },
                        new Error { Message = "OperationCanceledException", TranslationOfTheMessage = "Операция была отменена." },
                        new Error { Message = "ArgumentNullException", TranslationOfTheMessage = "Значение не может быть null." },
                        new Error { Message = "NotSupportedException", TranslationOfTheMessage = "Операция не поддерживается." },
                        new Error { Message = "ObjectDisposedException", TranslationOfTheMessage = "Невозможно получить доступ к уничтоженному объекту." },
                        new Error { Message = "ArithmeticException", TranslationOfTheMessage = "Произошла ошибка во время арифметических операций." },
                        new Error { Message = "DirectoryNotFoundException", TranslationOfTheMessage = "Указанный каталог не существует." },
                        new Error { Message = "PathTooLongException", TranslationOfTheMessage = "Указанный путь или имя файла слишком длинные." }
                    }
                },
                { Language.zh, new List<Error>
    {
        new Error { Message = "NullReferenceException", TranslationOfTheMessage = "对象引用未设置为对象的实例。" },
        new Error { Message = "ArgumentException", TranslationOfTheMessage = "提供了无效的参数。" },
        new Error { Message = "InvalidOperationException", TranslationOfTheMessage = "由于当前状态，操作无效。" },
        new Error { Message = "IndexOutOfRangeException", TranslationOfTheMessage = "索引超出了数组的边界。" },
        new Error { Message = "DivideByZeroException", TranslationOfTheMessage = "尝试除以零。" },
        new Error { Message = "FormatException", TranslationOfTheMessage = "输入字符串格式不正确。" },
        new Error { Message = "InvalidCastException", TranslationOfTheMessage = "指定的强制转换无效。" },
        new Error { Message = "NotImplementedException", TranslationOfTheMessage = "未实现该方法或操作。" },
        new Error { Message = "TimeoutException", TranslationOfTheMessage = "操作已超时。" },
        new Error { Message = "FileNotFoundException", TranslationOfTheMessage = "找不到指定的文件。" },
        new Error { Message = "UnauthorizedAccessException", TranslationOfTheMessage = "访问路径被拒绝。" },
        new Error { Message = "KeyNotFoundException", TranslationOfTheMessage = "找不到指定的键。" },
        new Error { Message = "OutOfMemoryException", TranslationOfTheMessage = "程序执行所需的内存不足。" },
        new Error { Message = "StackOverflowException", TranslationOfTheMessage = "请求的操作导致堆栈溢出。" },
        new Error { Message = "AccessViolationException", TranslationOfTheMessage = "尝试读取或写入受保护的内存。" },
        new Error { Message = "SqlException", TranslationOfTheMessage = "发生了 SQL Server 错误。" },
        new Error { Message = "IOException", TranslationOfTheMessage = "发生了输入输出错误。" },
        new Error { Message = "OperationCanceledException", TranslationOfTheMessage = "操作已被取消。" },
        new Error { Message = "ArgumentNullException", TranslationOfTheMessage = "值不能为 null。" },
        new Error { Message = "NotSupportedException", TranslationOfTheMessage = "不支持该操作。" },
        new Error { Message = "ObjectDisposedException", TranslationOfTheMessage = "无法访问已释放的对象。" },
        new Error { Message = "ArithmeticException", TranslationOfTheMessage = "在算术操作期间发生错误。" },
        new Error { Message = "DirectoryNotFoundException", TranslationOfTheMessage = "指定的目录不存在。" },
        new Error { Message = "PathTooLongException", TranslationOfTheMessage = "指定的路径或文件名太长。" }
    }
                },{ Language.ja, new List<Error>
                    {
                        new Error { Message = "NullReferenceException", TranslationOfTheMessage = "オブジェクト参照がオブジェクトのインスタンスに設定されていません。" },
                        new Error { Message = "ArgumentException", TranslationOfTheMessage = "無効な引数が提供されました。" },
                        new Error { Message = "InvalidOperationException", TranslationOfTheMessage = "現在の状態のため、操作は無効です。" },
                        new Error { Message = "IndexOutOfRangeException", TranslationOfTheMessage = "インデックスが配列の範囲外です。" },
                        new Error { Message = "DivideByZeroException", TranslationOfTheMessage = "ゼロで除算を試みました。" },
                        new Error { Message = "FormatException", TranslationOfTheMessage = "入力文字列の形式が正しくありません。" },
                        new Error { Message = "InvalidCastException", TranslationOfTheMessage = "指定されたキャストは無効です。" },
                        new Error { Message = "NotImplementedException", TranslationOfTheMessage = "メソッドまたは操作が実装されていません。" },
                        new Error { Message = "TimeoutException", TranslationOfTheMessage = "操作がタイムアウトしました。" },
                        new Error { Message = "FileNotFoundException", TranslationOfTheMessage = "指定されたファイルが見つかりません。" },
                        new Error { Message = "UnauthorizedAccessException", TranslationOfTheMessage = "パスへのアクセスが拒否されました。" },
                        new Error { Message = "KeyNotFoundException", TranslationOfTheMessage = "指定されたキーが見つかりません。" },
                        new Error { Message = "OutOfMemoryException", TranslationOfTheMessage = "プログラムの実行を続行するのに十分なメモリがありません。" },
                        new Error { Message = "StackOverflowException", TranslationOfTheMessage = "要求された操作によりスタックオーバーフローが発生しました。" },
                        new Error { Message = "AccessViolationException", TranslationOfTheMessage = "保護されたメモリの読み取りまたは書き込みを試みました。" },
                        new Error { Message = "SqlException", TranslationOfTheMessage = "SQL Serverエラーが発生しました。" },
                        new Error { Message = "IOException", TranslationOfTheMessage = "入出力エラーが発生しました。" },
                        new Error { Message = "OperationCanceledException", TranslationOfTheMessage = "操作がキャンセルされました。" },
                        new Error { Message = "ArgumentNullException", TranslationOfTheMessage = "値はnullにできません。" },
                        new Error { Message = "NotSupportedException", TranslationOfTheMessage = "操作はサポートされていません。" },
                        new Error { Message = "ObjectDisposedException", TranslationOfTheMessage = "解放されたオブジェクトにアクセスできません。" },
                        new Error { Message = "ArithmeticException", TranslationOfTheMessage = "算術演算中にエラーが発生しました。" },
                        new Error { Message = "DirectoryNotFoundException", TranslationOfTheMessage = "指定されたディレクトリは存在しません。" },
                        new Error { Message = "PathTooLongException", TranslationOfTheMessage = "指定されたパスまたはファイル名が長すぎます。" }
                    }
                },{ Language.ko, new List<Error>
                    {
                        new Error { Message = "NullReferenceException", TranslationOfTheMessage = "객체 참조가 개체의 인스턴스로 설정되어 있지 않습니다." },
                        new Error { Message = "ArgumentException", TranslationOfTheMessage = "유효하지 않은 인수가 제공되었습니다." },
                        new Error { Message = "InvalidOperationException", TranslationOfTheMessage = "현재 상태로 인해 작업이 유효하지 않습니다." },
                        new Error { Message = "IndexOutOfRangeException", TranslationOfTheMessage = "인덱스가 배열의 경계를 넘어섰습니다." },
                        new Error { Message = "DivideByZeroException", TranslationOfTheMessage = "0으로 나누기를 시도했습니다." },
                        new Error { Message = "FormatException", TranslationOfTheMessage = "입력 문자열의 형식이 올바르지 않습니다." },
                        new Error { Message = "InvalidCastException", TranslationOfTheMessage = "지정된 캐스트가 유효하지 않습니다." },
                        new Error { Message = "NotImplementedException", TranslationOfTheMessage = "메서드 또는 작업이 구현되지 않았습니다." },
                        new Error { Message = "TimeoutException", TranslationOfTheMessage = "작업이 시간 초과되었습니다." },
                        new Error { Message = "FileNotFoundException", TranslationOfTheMessage = "지정된 파일을 찾을 수 없습니다." },
                        new Error { Message = "UnauthorizedAccessException", TranslationOfTheMessage = "경로에 대한 액세스가 거부되었습니다." },
                        new Error { Message = "KeyNotFoundException", TranslationOfTheMessage = "지정된 키를 찾을 수 없습니다." },
                        new Error { Message = "OutOfMemoryException", TranslationOfTheMessage = "프로그램을 계속 실행할 충분한 메모리가 없습니다." },
                        new Error { Message = "StackOverflowException", TranslationOfTheMessage = "요청된 작업으로 인해 스택 오버플로우가 발생했습니다." },
                        new Error { Message = "AccessViolationException", TranslationOfTheMessage = "보호된 메모리를 읽거나 쓰려고 시도했습니다." },
                        new Error { Message = "SqlException", TranslationOfTheMessage = "SQL Server 오류가 발생했습니다." },
                        new Error { Message = "IOException", TranslationOfTheMessage = "입출력 오류가 발생했습니다." },
                        new Error { Message = "OperationCanceledException", TranslationOfTheMessage = "작업이 취소되었습니다." },
                        new Error { Message = "ArgumentNullException", TranslationOfTheMessage = "값은 null일 수 없습니다." },
                        new Error { Message = "NotSupportedException", TranslationOfTheMessage = "작업이 지원되지 않습니다." },
                        new Error { Message = "ObjectDisposedException", TranslationOfTheMessage = "해제된 개체에 접근할 수 없습니다." },
                        new Error { Message = "ArithmeticException", TranslationOfTheMessage = "산술 연산 중 오류가 발생했습니다." },
                        new Error { Message = "DirectoryNotFoundException", TranslationOfTheMessage = "지정된 디렉터리가 존재하지 않습니다." },
                        new Error { Message = "PathTooLongException", TranslationOfTheMessage = "지정된 경로 또는 파일 이름이 너무 깁니다." }
                    }
                },
                { Language.pt, new List<Error>
                    {
                        new Error { Message = "NullReferenceException", TranslationOfTheMessage = "A referência de objeto não está definida como uma instância de um objeto." },
                        new Error { Message = "ArgumentException", TranslationOfTheMessage = "Argumento inválido fornecido." },
                        new Error { Message = "InvalidOperationException", TranslationOfTheMessage = "A operação não é válida devido ao estado atual." },
                        new Error { Message = "IndexOutOfRangeException", TranslationOfTheMessage = "O índice estava fora dos limites do array." },
                        new Error { Message = "DivideByZeroException", TranslationOfTheMessage = "Tentativa de dividir por zero." },
                        new Error { Message = "FormatException", TranslationOfTheMessage = "A string de entrada não estava em um formato correto." },
                        new Error { Message = "InvalidCastException", TranslationOfTheMessage = "O cast especificado não é válido." },
                        new Error { Message = "NotImplementedException", TranslationOfTheMessage = "O método ou a operação não está implementado." },
                        new Error { Message = "TimeoutException", TranslationOfTheMessage = "A operação expirou." },
                        new Error { Message = "FileNotFoundException", TranslationOfTheMessage = "O arquivo especificado não foi encontrado." },
                        new Error { Message = "UnauthorizedAccessException", TranslationOfTheMessage = "Acesso ao caminho é negado." },
                        new Error { Message = "KeyNotFoundException", TranslationOfTheMessage = "A chave especificada não foi encontrada." },
                        new Error { Message = "OutOfMemoryException", TranslationOfTheMessage = "Memória insuficiente para continuar a execução do programa." },
                        new Error { Message = "StackOverflowException", TranslationOfTheMessage = "A operação solicitada causou um estouro de pilha." },
                        new Error { Message = "AccessViolationException", TranslationOfTheMessage = "Tentativa de ler ou gravar em memória protegida." },
                        new Error { Message = "SqlException", TranslationOfTheMessage = "Ocorreu um erro no SQL Server." },
                        new Error { Message = "IOException", TranslationOfTheMessage = "Ocorreu um erro de I/O." },
                        new Error { Message = "OperationCanceledException", TranslationOfTheMessage = "A operação foi cancelada." },
                        new Error { Message = "ArgumentNullException", TranslationOfTheMessage = "O valor não pode ser nulo." },
                        new Error { Message = "NotSupportedException", TranslationOfTheMessage = "A operação não é suportada." },
                        new Error { Message = "ObjectDisposedException", TranslationOfTheMessage = "Não é possível acessar um objeto descartado." },
                        new Error { Message = "ArithmeticException", TranslationOfTheMessage = "Ocorreu um erro durante operações aritméticas." },
                        new Error { Message = "DirectoryNotFoundException", TranslationOfTheMessage = "O diretório especificado não existe." },
                        new Error { Message = "PathTooLongException", TranslationOfTheMessage = "O caminho ou o nome do arquivo especificado é muito longo." }
                    }
                },
                { Language.ar, new List<Error>
                    {
                        new Error { Message = "NullReferenceException", TranslationOfTheMessage = "مرجع الكائن غير مضبوط كحالة لكائن." },
                        new Error { Message = "ArgumentException", TranslationOfTheMessage = "تم تقديم حجة غير صالحة." },
                        new Error { Message = "InvalidOperationException", TranslationOfTheMessage = "العملية غير صالحة بسبب الحالة الحالية." },
                        new Error { Message = "IndexOutOfRangeException", TranslationOfTheMessage = "كان الفهرس خارج حدود المصفوفة." },
                        new Error { Message = "DivideByZeroException", TranslationOfTheMessage = "تمت محاولة القسمة على صفر." },
                        new Error { Message = "FormatException", TranslationOfTheMessage = "سلسلة الإدخال لم تكن بتنسيق صحيح." },
                        new Error { Message = "InvalidCastException", TranslationOfTheMessage = "التحويل المحدد غير صالح." },
                        new Error { Message = "NotImplementedException", TranslationOfTheMessage = "لم يتم تنفيذ الطريقة أو العملية." },
                        new Error { Message = "TimeoutException", TranslationOfTheMessage = "انتهت صلاحية العملية." },
                        new Error { Message = "FileNotFoundException", TranslationOfTheMessage = "لم يتم العثور على الملف المحدد." },
                        new Error { Message = "UnauthorizedAccessException", TranslationOfTheMessage = "تم رفض الوصول إلى المسار." },
                        new Error { Message = "KeyNotFoundException", TranslationOfTheMessage = "لم يتم العثور على المفتاح المحدد." },
                        new Error { Message = "OutOfMemoryException", TranslationOfTheMessage = "ذاكرة غير كافية لمتابعة تنفيذ البرنامج." },
                        new Error { Message = "StackOverflowException", TranslationOfTheMessage = "العملية المطلوبة تسببت في تجاوز الحد الأقصى للذاكرة." },
                        new Error { Message = "AccessViolationException", TranslationOfTheMessage = "تمت محاولة القراءة أو الكتابة في ذاكرة محمية." },
                        new Error { Message = "SqlException", TranslationOfTheMessage = "حدث خطأ في خادم SQL." },
                        new Error { Message = "IOException", TranslationOfTheMessage = "حدث خطأ في الإدخال / الإخراج." },
                        new Error { Message = "OperationCanceledException", TranslationOfTheMessage = "تم إلغاء العملية." },
                        new Error { Message = "ArgumentNullException", TranslationOfTheMessage = "لا يمكن أن يكون القيمة فارغة." },
                        new Error { Message = "NotSupportedException", TranslationOfTheMessage = "العملية غير مدعومة." },
                        new Error { Message = "ObjectDisposedException", TranslationOfTheMessage = "لا يمكن الوصول إلى كائن تم التخلص منه." },
                        new Error { Message = "ArithmeticException", TranslationOfTheMessage = "حدث خطأ أثناء العمليات الحسابية." },
                        new Error { Message = "DirectoryNotFoundException", TranslationOfTheMessage = "الدليل المحدد غير موجود." },
                        new Error { Message = "PathTooLongException", TranslationOfTheMessage = "المسار أو اسم الملف المحدد طويل جداً." }
                    }
                },{ Language.hi, new List<Error>
                    {
                        new Error { Message = "NullReferenceException", TranslationOfTheMessage = "ऑब्जेक्ट संदर्भ किसी ऑब्जेक्ट के उदाहरण के लिए सेट नहीं है।" },
                        new Error { Message = "ArgumentException", TranslationOfTheMessage = "अमान्य तर्क प्रदान किया गया।" },
                        new Error { Message = "InvalidOperationException", TranslationOfTheMessage = "वर्तमान स्थिति के कारण संचालन अमान्य है।" },
                        new Error { Message = "IndexOutOfRangeException", TranslationOfTheMessage = "सूचकांक सरणी की सीमाओं से बाहर था।" },
                        new Error { Message = "DivideByZeroException", TranslationOfTheMessage = "शून्य से विभाजन करने का प्रयास किया गया।" },
                        new Error { Message = "FormatException", TranslationOfTheMessage = "इनपुट स्ट्रिंग सही प्रारूप में नहीं थी।" },
                        new Error { Message = "InvalidCastException", TranslationOfTheMessage = "निर्दिष्ट CAST मान्य नहीं है।" },
                        new Error { Message = "NotImplementedException", TranslationOfTheMessage = "तरीका या संचालन लागू नहीं किया गया।" },
                        new Error { Message = "TimeoutException", TranslationOfTheMessage = "संचालन समय सीमा समाप्त हो गई।" },
                        new Error { Message = "FileNotFoundException", TranslationOfTheMessage = "निर्दिष्ट फ़ाइल नहीं मिली।" },
                        new Error { Message = "UnauthorizedAccessException", TranslationOfTheMessage = "पथ तक पहुंच अस्वीकृत है।" },
                        new Error { Message = "KeyNotFoundException", TranslationOfTheMessage = "निर्दिष्ट कुंजी नहीं मिली।" },
                        new Error { Message = "OutOfMemoryException", TranslationOfTheMessage = "कार्यक्रम के निष्पादन को जारी रखने के लिए पर्याप्त मेमोरी नहीं है।" },
                        new Error { Message = "StackOverflowException", TranslationOfTheMessage = "अनुरोधित संचालन ने स्टैक ओवरफ्लो किया।" },
                        new Error { Message = "AccessViolationException", TranslationOfTheMessage = "संरक्षित मेमोरी को पढ़ने या लिखने का प्रयास किया गया।" },
                        new Error { Message = "SqlException", TranslationOfTheMessage = "SQL सर्वर में एक त्रुटि हुई।" },
                        new Error { Message = "IOException", TranslationOfTheMessage = "इनपुट/आउटपुट त्रुटि हुई।" },
                        new Error { Message = "OperationCanceledException", TranslationOfTheMessage = "संचालन रद्द कर दिया गया।" },
                        new Error { Message = "ArgumentNullException", TranslationOfTheMessage = "मान शून्य नहीं हो सकता।" },
                        new Error { Message = "NotSupportedException", TranslationOfTheMessage = "संचालन समर्थित नहीं है।" },
                        new Error { Message = "ObjectDisposedException", TranslationOfTheMessage = "निर्धारित वस्तु तक पहुँच नहीं की जा सकती।" },
                        new Error { Message = "ArithmeticException", TranslationOfTheMessage = "गणितीय संचालन के दौरान एक त्रुटि हुई।" },
                        new Error { Message = "DirectoryNotFoundException", TranslationOfTheMessage = "निर्दिष्ट निर्देशिका मौजूद नहीं है।" },
                        new Error { Message = "PathTooLongException", TranslationOfTheMessage = "निर्दिष्ट पथ या फ़ाइल नाम बहुत लंबा है।" }
                    }
                },
                { Language.pl, new List<Error>
                    {
                        new Error { Message = "NullReferenceException", TranslationOfTheMessage = "Referencja obiektu nie jest ustawiona na instancję obiektu." },
                        new Error { Message = "ArgumentException", TranslationOfTheMessage = "Podano nieprawidłowy argument." },
                        new Error { Message = "InvalidOperationException", TranslationOfTheMessage = "Operacja jest nieprawidłowa z powodu bieżącego stanu." },
                        new Error { Message = "IndexOutOfRangeException", TranslationOfTheMessage = "Indeks był poza granicami tablicy." },
                        new Error { Message = "DivideByZeroException", TranslationOfTheMessage = "Podjęto próbę podziału przez zero." },
                        new Error { Message = "FormatException", TranslationOfTheMessage = "Wprowadzony ciąg nie był w poprawnym formacie." },
                        new Error { Message = "InvalidCastException", TranslationOfTheMessage = "Określony rzut jest nieprawidłowy." },
                        new Error { Message = "NotImplementedException", TranslationOfTheMessage = "Metoda lub operacja nie została zaimplementowana." },
                        new Error { Message = "TimeoutException", TranslationOfTheMessage = "Operacja przekroczyła czas." },
                        new Error { Message = "FileNotFoundException", TranslationOfTheMessage = "Nie znaleziono określonego pliku." },
                        new Error { Message = "UnauthorizedAccessException", TranslationOfTheMessage = "Dostęp do ścieżki jest zabroniony." },
                        new Error { Message = "KeyNotFoundException", TranslationOfTheMessage = "Nie znaleziono określonego klucza." },
                        new Error { Message = "OutOfMemoryException", TranslationOfTheMessage = "Brak wystarczającej pamięci, aby kontynuować wykonywanie programu." },
                        new Error { Message = "StackOverflowException", TranslationOfTheMessage = "Żądana operacja spowodowała przepełnienie stosu." },
                        new Error { Message = "AccessViolationException", TranslationOfTheMessage = "Podjęto próbę odczytu lub zapisu w chronionej pamięci." },
                        new Error { Message = "SqlException", TranslationOfTheMessage = "Wystąpił błąd serwera SQL." },
                        new Error { Message = "IOException", TranslationOfTheMessage = "Wystąpił błąd I/O." },
                        new Error { Message = "OperationCanceledException", TranslationOfTheMessage = "Operacja została anulowana." },
                        new Error { Message = "ArgumentNullException", TranslationOfTheMessage = "Wartość nie może być pusta." },
                        new Error { Message = "NotSupportedException", TranslationOfTheMessage = "Operacja nie jest obsługiwana." },
                        new Error { Message = "ObjectDisposedException", TranslationOfTheMessage = "Nie można uzyskać dostępu do usuniętego obiektu." },
                        new Error { Message = "ArithmeticException", TranslationOfTheMessage = "Wystąpił błąd podczas operacji arytmetycznych." },
                        new Error { Message = "DirectoryNotFoundException", TranslationOfTheMessage = "Określony katalog nie istnieje." },
                        new Error { Message = "PathTooLongException", TranslationOfTheMessage = "Określona ścieżka lub nazwa pliku jest zbyt długa." }
                    }
                },
                { Language.tr, new List<Error>
                    {
                        new Error { Message = "NullReferenceException", TranslationOfTheMessage = "Nesne referansı bir nesnenin örneğine ayarlanmamış." },
                        new Error { Message = "ArgumentException", TranslationOfTheMessage = "Geçersiz bir argüman sağlandı." },
                        new Error { Message = "InvalidOperationException", TranslationOfTheMessage = "Mevcut durum nedeniyle işlem geçersiz." },
                        new Error { Message = "IndexOutOfRangeException", TranslationOfTheMessage = "İndeks dizinin sınırlarının dışındaydı." },
                        new Error { Message = "DivideByZeroException", TranslationOfTheMessage = "Sıfıra bölme girişiminde bulunuldu." },
                        new Error { Message = "FormatException", TranslationOfTheMessage = "Girdi dizesi doğru bir formatta değildi." },
                        new Error { Message = "InvalidCastException", TranslationOfTheMessage = "Belirtilen dönüşüm geçerli değil." },
                        new Error { Message = "NotImplementedException", TranslationOfTheMessage = "Metot veya işlem uygulanmamış." },
                        new Error { Message = "TimeoutException", TranslationOfTheMessage = "İşlem zaman aşımına uğradı." },
                        new Error { Message = "FileNotFoundException", TranslationOfTheMessage = "Belirtilen dosya bulunamadı." },
                        new Error { Message = "UnauthorizedAccessException", TranslationOfTheMessage = "Yola erişim reddedildi." },
                        new Error { Message = "KeyNotFoundException", TranslationOfTheMessage = "Belirtilen anahtar bulunamadı." },
                        new Error { Message = "OutOfMemoryException", TranslationOfTheMessage = "Programın yürütülmesini sürdürmek için yeterli bellek yok." },
                        new Error { Message = "StackOverflowException", TranslationOfTheMessage = "İstenilen işlem bir yığın taşmasına neden oldu." },
                        new Error { Message = "AccessViolationException", TranslationOfTheMessage = "Korunan belleğe okuma veya yazma girişiminde bulunuldu." },
                        new Error { Message = "SqlException", TranslationOfTheMessage = "SQL Server hatası oluştu." },
                        new Error { Message = "IOException", TranslationOfTheMessage = "G/Ç hatası oluştu." },
                        new Error { Message = "OperationCanceledException", TranslationOfTheMessage = "İşlem iptal edildi." },
                        new Error { Message = "ArgumentNullException", TranslationOfTheMessage = "Değer null olamaz." },
                        new Error { Message = "NotSupportedException", TranslationOfTheMessage = "İşlem desteklenmiyor." },
                        new Error { Message = "ObjectDisposedException", TranslationOfTheMessage = "Serbest bırakılmış bir nesneye erişilemez." },
                        new Error { Message = "ArithmeticException", TranslationOfTheMessage = "Aritmetik işlemler sırasında bir hata oluştu." },
                        new Error { Message = "DirectoryNotFoundException", TranslationOfTheMessage = "Belirtilen dizin mevcut değil." },
                        new Error { Message = "PathTooLongException", TranslationOfTheMessage = "Belirtilen yol veya dosya adı çok uzun." }
                    }
                },
                { Language.nl, new List<Error>
                    {
                        new Error { Message = "NullReferenceException", TranslationOfTheMessage = "Objectreferentie is niet ingesteld op een exemplaar van een object." },
                        new Error { Message = "ArgumentException", TranslationOfTheMessage = "Ongeldig argument opgegeven." },
                        new Error { Message = "InvalidOperationException", TranslationOfTheMessage = "De bewerking is niet geldig vanwege de huidige toestand." },
                        new Error { Message = "IndexOutOfRangeException", TranslationOfTheMessage = "Index was buiten de grenzen van de array." },
                        new Error { Message = "DivideByZeroException", TranslationOfTheMessage = "Poging om door nul te delen." },
                        new Error { Message = "FormatException", TranslationOfTheMessage = "De invoerreeks was niet in een correct formaat." },
                        new Error { Message = "InvalidCastException", TranslationOfTheMessage = "Opgegeven cast is niet geldig." },
                        new Error { Message = "NotImplementedException", TranslationOfTheMessage = "De methode of bewerking is niet geïmplementeerd." },
                        new Error { Message = "TimeoutException", TranslationOfTheMessage = "De bewerking is verlopen." },
                        new Error { Message = "FileNotFoundException", TranslationOfTheMessage = "Het opgegeven bestand is niet gevonden." },
                        new Error { Message = "UnauthorizedAccessException", TranslationOfTheMessage = "Toegang tot het pad is geweigerd." },
                        new Error { Message = "KeyNotFoundException", TranslationOfTheMessage = "De opgegeven sleutel is niet gevonden." },
                        new Error { Message = "OutOfMemoryException", TranslationOfTheMessage = "Onvoldoende geheugen om de uitvoering van het programma voort te zetten." },
                        new Error { Message = "StackOverflowException", TranslationOfTheMessage = "De gevraagde bewerking veroorzaakte een stackoverloop." },
                        new Error { Message = "AccessViolationException", TranslationOfTheMessage = "Poging om te lezen of te schrijven naar beschermde geheugen." },
                        new Error { Message = "SqlException", TranslationOfTheMessage = "Er is een SQL Server-fout opgetreden." },
                        new Error { Message = "IOException", TranslationOfTheMessage = "Er is een I/O-fout opgetreden." },
                        new Error { Message = "OperationCanceledException", TranslationOfTheMessage = "De bewerking is geannuleerd." },
                        new Error { Message = "ArgumentNullException", TranslationOfTheMessage = "Waarde mag niet null zijn." },
                        new Error { Message = "NotSupportedException", TranslationOfTheMessage = "De bewerking wordt niet ondersteund." },
                        new Error { Message = "ObjectDisposedException", TranslationOfTheMessage = "Kan geen toegang krijgen tot een vrijgegeven object." },
                        new Error { Message = "ArithmeticException", TranslationOfTheMessage = "Er is een fout opgetreden tijdens rekenkundige bewerkingen." },
                        new Error { Message = "DirectoryNotFoundException", TranslationOfTheMessage = "De opgegeven map bestaat niet." },
                        new Error { Message = "PathTooLongException", TranslationOfTheMessage = "Het opgegeven pad of bestandsnaam is te lang." }
                    }
                },
                { Language.sv, new List<Error>
                    {
                        new Error { Message = "NullReferenceException", TranslationOfTheMessage = "Objektreferens har inte angetts till en instans av ett objekt." },
                        new Error { Message = "ArgumentException", TranslationOfTheMessage = "Ogiltigt argument angivet." },
                        new Error { Message = "InvalidOperationException", TranslationOfTheMessage = "Operationen är inte giltig på grund av det aktuella tillståndet." },
                        new Error { Message = "IndexOutOfRangeException", TranslationOfTheMessage = "Index var utanför arrayens gränser." },
                        new Error { Message = "DivideByZeroException", TranslationOfTheMessage = "Försökte dela med noll." },
                        new Error { Message = "FormatException", TranslationOfTheMessage = "Inmatningssträngen var inte i ett korrekt format." },
                        new Error { Message = "InvalidCastException", TranslationOfTheMessage = "Den angivna castningen är inte giltig." },
                        new Error { Message = "NotImplementedException", TranslationOfTheMessage = "Metoden eller operationen är inte implementerad." },
                        new Error { Message = "TimeoutException", TranslationOfTheMessage = "Operationen har tidsöverskridit." },
                        new Error { Message = "FileNotFoundException", TranslationOfTheMessage = "Den angivna filen hittades inte." },
                        new Error { Message = "UnauthorizedAccessException", TranslationOfTheMessage = "Åtkomst till sökvägen nekades." },
                        new Error { Message = "KeyNotFoundException", TranslationOfTheMessage = "Den angivna nyckeln hittades inte." },
                        new Error { Message = "OutOfMemoryException", TranslationOfTheMessage = "Otillräckligt minne för att fortsätta programmets exekvering." },
                        new Error { Message = "StackOverflowException", TranslationOfTheMessage = "Den begärda operationen orsakade en stacköverflöd." },
                        new Error { Message = "AccessViolationException", TranslationOfTheMessage = "Försökte läsa eller skriva skyddad minne." },
                        new Error { Message = "SqlException", TranslationOfTheMessage = "Ett SQL Server-fel inträffade." },
                        new Error { Message = "IOException", TranslationOfTheMessage = "Ett I/O-fel inträffade." },
                        new Error { Message = "OperationCanceledException", TranslationOfTheMessage = "Operationen har avbrutits." },
                        new Error { Message = "ArgumentNullException", TranslationOfTheMessage = "Värdet kan inte vara null." },
                        new Error { Message = "NotSupportedException", TranslationOfTheMessage = "Operationen stöds inte." },
                        new Error { Message = "ObjectDisposedException", TranslationOfTheMessage = "Kan inte komma åt ett borttaget objekt." },
                        new Error { Message = "ArithmeticException", TranslationOfTheMessage = "Ett fel inträffade under aritmetiska operationer." },
                        new Error { Message = "DirectoryNotFoundException", TranslationOfTheMessage = "Den angivna katalogen finns inte." },
                        new Error { Message = "PathTooLongException", TranslationOfTheMessage = "Den angivna sökvägen eller filnamnet är för långt." }
                    }
                },
                { Language.da, new List<Error>
                    {
                        new Error { Message = "NullReferenceException", TranslationOfTheMessage = "Objektreferencen er ikke indstillet til en instans af et objekt." },
                        new Error { Message = "ArgumentException", TranslationOfTheMessage = "Ugyldigt argument angivet." },
                        new Error { Message = "InvalidOperationException", TranslationOfTheMessage = "Operationen er ikke gyldig på grund af den nuværende tilstand." },
                        new Error { Message = "IndexOutOfRangeException", TranslationOfTheMessage = "Indekset var uden for arrayens grænser." },
                        new Error { Message = "DivideByZeroException", TranslationOfTheMessage = "Forsøgte at dividere med nul." },
                        new Error { Message = "FormatException", TranslationOfTheMessage = "Indtastningsstrengen var ikke i et korrekt format." },
                        new Error { Message = "InvalidCastException", TranslationOfTheMessage = "Den angivne typekonvertering er ikke gyldig." },
                        new Error { Message = "NotImplementedException", TranslationOfTheMessage = "Metoden eller operationen er ikke implementeret." },
                        new Error { Message = "TimeoutException", TranslationOfTheMessage = "Operationen har overskredet tidsgrænsen." },
                        new Error { Message = "FileNotFoundException", TranslationOfTheMessage = "Den angivne fil blev ikke fundet." },
                        new Error { Message = "UnauthorizedAccessException", TranslationOfTheMessage = "Adgang til stien blev nægtet." },
                        new Error { Message = "KeyNotFoundException", TranslationOfTheMessage = "Den angivne nøgle blev ikke fundet." },
                        new Error { Message = "OutOfMemoryException", TranslationOfTheMessage = "Utilstrækkelig hukommelse til at fortsætte programmets udførelse." },
                        new Error { Message = "StackOverflowException", TranslationOfTheMessage = "Den anmodede operation forårsagede et stakoverløb." },
                        new Error { Message = "AccessViolationException", TranslationOfTheMessage = "Forsøgte at læse eller skrive til beskyttet hukommelse." },
                        new Error { Message = "SqlException", TranslationOfTheMessage = "Der opstod en SQL Server-fejl." },
                        new Error { Message = "IOException", TranslationOfTheMessage = "Der opstod en I/O-fejl." },
                        new Error { Message = "OperationCanceledException", TranslationOfTheMessage = "Operationen er blevet annulleret." },
                        new Error { Message = "ArgumentNullException", TranslationOfTheMessage = "Værdien må ikke være null." },
                        new Error { Message = "NotSupportedException", TranslationOfTheMessage = "Operationen understøttes ikke." },
                        new Error { Message = "ObjectDisposedException", TranslationOfTheMessage = "Kan ikke få adgang til et bortskilt objekt." },
                        new Error { Message = "ArithmeticException", TranslationOfTheMessage = "Der opstod en fejl under aritmetiske operationer." },
                        new Error { Message = "DirectoryNotFoundException", TranslationOfTheMessage = "Den angivne mappe findes ikke." },
                        new Error { Message = "PathTooLongException", TranslationOfTheMessage = "Den angivne sti eller filnavn er for langt." }
                    }
                },
                { Language.fi, new List<Error>
                    {
                        new Error { Message = "NullReferenceException", TranslationOfTheMessage = "Objektiviite ei ole asetettu olion instanssiin." },
                        new Error { Message = "ArgumentException", TranslationOfTheMessage = "Virheellinen argumentti annettu." },
                        new Error { Message = "InvalidOperationException", TranslationOfTheMessage = "Operaatio ei ole voimassa nykyisessä tilassa." },
                        new Error { Message = "IndexOutOfRangeException", TranslationOfTheMessage = "Indeksi oli taulukon rajojen ulkopuolella." },
                        new Error { Message = "DivideByZeroException", TranslationOfTheMessage = "Yrittäminen jakaa nollalla." },
                        new Error { Message = "FormatException", TranslationOfTheMessage = "Syöte ei ollut oikeassa muodossa." },
                        new Error { Message = "InvalidCastException", TranslationOfTheMessage = "Määritetty tyyppimuunnos ei ole voimassa." },
                        new Error { Message = "NotImplementedException", TranslationOfTheMessage = "Menetelmää tai toimintoa ei ole toteutettu." },
                        new Error { Message = "TimeoutException", TranslationOfTheMessage = "Toiminto aikakatkaistiin." },
                        new Error { Message = "FileNotFoundException", TranslationOfTheMessage = "Määritettyä tiedostoa ei löytynyt." },
                        new Error { Message = "UnauthorizedAccessException", TranslationOfTheMessage = "Pääsy polkuun evättiin." },
                        new Error { Message = "KeyNotFoundException", TranslationOfTheMessage = "Määritettyä avainta ei löytynyt." },
                        new Error { Message = "OutOfMemoryException", TranslationOfTheMessage = "Riittämätön muisti ohjelman suorittamiseen." },
                        new Error { Message = "StackOverflowException", TranslationOfTheMessage = "Pyydetty operaatio aiheutti pinon ylivuodon." },
                        new Error { Message = "AccessViolationException", TranslationOfTheMessage = "Yrittäminen lukea tai kirjoittaa suojattuun muistiin." },
                        new Error { Message = "SqlException", TranslationOfTheMessage = "SQL Server -virhe tapahtui." },
                        new Error { Message = "IOException", TranslationOfTheMessage = "I/O-virhe tapahtui." },
                        new Error { Message = "OperationCanceledException", TranslationOfTheMessage = "Toiminto on peruutettu." },
                        new Error { Message = "ArgumentNullException", TranslationOfTheMessage = "Arvo ei voi olla null." },
                        new Error { Message = "NotSupportedException", TranslationOfTheMessage = "Toimintoa ei tueta." },
                        new Error { Message = "ObjectDisposedException", TranslationOfTheMessage = "Ei voida käyttää hävitettyä objektia." },
                        new Error { Message = "ArithmeticException", TranslationOfTheMessage = "Virhe tapahtui matemaattisissa operaatioissa." },
                        new Error { Message = "DirectoryNotFoundException", TranslationOfTheMessage = "Määritettyä hakemistoa ei löytynyt." },
                        new Error { Message = "PathTooLongException", TranslationOfTheMessage = "Määritetty polku tai tiedostonimi on liian pitkä." }
                    }
                },
                { Language.no, new List<Error>
                    {
                        new Error { Message = "NullReferenceException", TranslationOfTheMessage = "Objektreferansen er ikke satt til en instans av et objekt." },
                        new Error { Message = "ArgumentException", TranslationOfTheMessage = "Ugyldig argument angitt." },
                        new Error { Message = "InvalidOperationException", TranslationOfTheMessage = "Operasjonen er ikke gyldig på grunn av den nåværende tilstanden." },
                        new Error { Message = "IndexOutOfRangeException", TranslationOfTheMessage = "Indeksen var utenfor arrayens grenser." },
                        new Error { Message = "DivideByZeroException", TranslationOfTheMessage = "Forsøk på å dele med null." },
                        new Error { Message = "FormatException", TranslationOfTheMessage = "Inndataene var ikke i et korrekt format." },
                        new Error { Message = "InvalidCastException", TranslationOfTheMessage = "Den angitte typekonverteringen er ikke gyldig." },
                        new Error { Message = "NotImplementedException", TranslationOfTheMessage = "Metoden eller operasjonen er ikke implementert." },
                        new Error { Message = "TimeoutException", TranslationOfTheMessage = "Operasjonen har tidsavbrudd." },
                        new Error { Message = "FileNotFoundException", TranslationOfTheMessage = "Den angitte filen ble ikke funnet." },
                        new Error { Message = "UnauthorizedAccessException", TranslationOfTheMessage = "Tilgang til stien ble nektet." },
                        new Error { Message = "KeyNotFoundException", TranslationOfTheMessage = "Den angitte nøkkelen ble ikke funnet." },
                        new Error { Message = "OutOfMemoryException", TranslationOfTheMessage = "Utilstrekkelig minne til å fortsette programmets utførelse." },
                        new Error { Message = "StackOverflowException", TranslationOfTheMessage = "Den forespurte operasjonen forårsaket et stakkoverløp." },
                        new Error { Message = "AccessViolationException", TranslationOfTheMessage = "Forsøk på å lese eller skrive til beskyttet minne." },
                        new Error { Message = "SqlException", TranslationOfTheMessage = "En SQL Server-feil oppstod." },
                        new Error { Message = "IOException", TranslationOfTheMessage = "Det oppstod en I/O-feil." },
                        new Error { Message = "OperationCanceledException", TranslationOfTheMessage = "Operasjonen ble kansellert." },
                        new Error { Message = "ArgumentNullException", TranslationOfTheMessage = "Verdien kan ikke være null." },
                        new Error { Message = "NotSupportedException", TranslationOfTheMessage = "Operasjonen støttes ikke." },
                        new Error { Message = "ObjectDisposedException", TranslationOfTheMessage = "Kan ikke få tilgang til et avhendet objekt." },
                        new Error { Message = "ArithmeticException", TranslationOfTheMessage = "En feil oppstod under aritmetiske operasjoner." },
                        new Error { Message = "DirectoryNotFoundException", TranslationOfTheMessage = "Den angitte mappen ble ikke funnet." },
                        new Error { Message = "PathTooLongException", TranslationOfTheMessage = "Den angitte stien eller filnavnet er for langt." }
                    }
                },
                { Language.cs, new List<Error>
                    {
                        new Error { Message = "NullReferenceException", TranslationOfTheMessage = "Odkaz na objekt není nastaven na instanci objektu." },
                        new Error { Message = "ArgumentException", TranslationOfTheMessage = "Byl zadán neplatný argument." },
                        new Error { Message = "InvalidOperationException", TranslationOfTheMessage = "Operace není platná vzhledem k aktuálnímu stavu." },
                        new Error { Message = "IndexOutOfRangeException", TranslationOfTheMessage = "Index byl mimo meze pole." },
                        new Error { Message = "DivideByZeroException", TranslationOfTheMessage = "Pokoušíte se dělit nulou." },
                        new Error { Message = "FormatException", TranslationOfTheMessage = "Vstupní řetězec nebyl ve správném formátu." },
                        new Error { Message = "InvalidCastException", TranslationOfTheMessage = "Určená konverze typu není platná." },
                        new Error { Message = "NotImplementedException", TranslationOfTheMessage = "Metoda nebo operace není implementována." },
                        new Error { Message = "TimeoutException", TranslationOfTheMessage = "Operace překročila časový limit." },
                        new Error { Message = "FileNotFoundException", TranslationOfTheMessage = "Zadaný soubor nebyl nalezen." },
                        new Error { Message = "UnauthorizedAccessException", TranslationOfTheMessage = "Přístup k cestě byl odepřen." },
                        new Error { Message = "KeyNotFoundException", TranslationOfTheMessage = "Zadaný klíč nebyl nalezen." },
                        new Error { Message = "OutOfMemoryException", TranslationOfTheMessage = "Nedostatek paměti pro pokračování vykonávání programu." },
                        new Error { Message = "StackOverflowException", TranslationOfTheMessage = "Požadovaná operace způsobila přetečení zásobníku." },
                        new Error { Message = "AccessViolationException", TranslationOfTheMessage = "Pokoušíte se číst nebo zapisovat do chráněné paměti." },
                        new Error { Message = "SqlException", TranslationOfTheMessage = "Došlo k chybě SQL Serveru." },
                        new Error { Message = "IOException", TranslationOfTheMessage = "Došlo k chybě I/O." },
                        new Error { Message = "OperationCanceledException", TranslationOfTheMessage = "Operace byla zrušena." },
                        new Error { Message = "ArgumentNullException", TranslationOfTheMessage = "Hodnota nemůže být null." },
                        new Error { Message = "NotSupportedException", TranslationOfTheMessage = "Operace není podporována." },
                        new Error { Message = "ObjectDisposedException", TranslationOfTheMessage = "Nelze přistupovat k zlikvidovanému objektu." },
                        new Error { Message = "ArithmeticException", TranslationOfTheMessage = "Došlo k chybě během aritmetických operací." },
                        new Error { Message = "DirectoryNotFoundException", TranslationOfTheMessage = "Zadaný adresář nebyl nalezen." },
                        new Error { Message = "PathTooLongException", TranslationOfTheMessage = "Zadaná cesta nebo název souboru je příliš dlouhý." }
                    }
                },
                { Language.hu, new List<Error>
                    {
                        new Error { Message = "NullReferenceException", TranslationOfTheMessage = "Az objektum referencia nincs beállítva egy objektum példányára." },
                        new Error { Message = "ArgumentException", TranslationOfTheMessage = "Érvénytelen argumentum került megadásra." },
                        new Error { Message = "InvalidOperationException", TranslationOfTheMessage = "A művelet nem érvényes a jelenlegi állapot miatt." },
                        new Error { Message = "IndexOutOfRangeException", TranslationOfTheMessage = "Az index a tömb határain kívül esik." },
                        new Error { Message = "DivideByZeroException", TranslationOfTheMessage = "Zéróval próbált osztani." },
                        new Error { Message = "FormatException", TranslationOfTheMessage = "A bemeneti karakterlánc nem volt helyes formátumú." },
                        new Error { Message = "InvalidCastException", TranslationOfTheMessage = "A megadott típus átkonvertálása nem érvényes." },
                        new Error { Message = "NotImplementedException", TranslationOfTheMessage = "A módszer vagy művelet nincs implementálva." },
                        new Error { Message = "TimeoutException", TranslationOfTheMessage = "A művelet időkorlátot lépett át." },
                        new Error { Message = "FileNotFoundException", TranslationOfTheMessage = "A megadott fájl nem található." },
                        new Error { Message = "UnauthorizedAccessException", TranslationOfTheMessage = "A hozzáférés a megadott úthoz elutasítva." },
                        new Error { Message = "KeyNotFoundException", TranslationOfTheMessage = "A megadott kulcs nem található." },
                        new Error { Message = "OutOfMemoryException", TranslationOfTheMessage = "Nincs elegendő memória a program végrehajtásához." },
                        new Error { Message = "StackOverflowException", TranslationOfTheMessage = "A kért művelet verem túlcsordulást okozott." },
                        new Error { Message = "AccessViolationException", TranslationOfTheMessage = "Próbálkozás olvasni vagy írni védett memóriába." },
                        new Error { Message = "SqlException", TranslationOfTheMessage = "SQL Server hiba történt." },
                        new Error { Message = "IOException", TranslationOfTheMessage = "Bemeneti/kimeneti hiba történt." },
                        new Error { Message = "OperationCanceledException", TranslationOfTheMessage = "A műveletet törölték." },
                        new Error { Message = "ArgumentNullException", TranslationOfTheMessage = "Az érték nem lehet null." },
                        new Error { Message = "NotSupportedException", TranslationOfTheMessage = "A művelet nem támogatott." },
                        new Error { Message = "ObjectDisposedException", TranslationOfTheMessage = "Nem lehet hozzáférni a felszabadított objektumhoz." },
                        new Error { Message = "ArithmeticException", TranslationOfTheMessage = "Hiba történt a matematikai műveletek során." },
                        new Error { Message = "DirectoryNotFoundException", TranslationOfTheMessage = "A megadott könyvtár nem található." },
                        new Error { Message = "PathTooLongException", TranslationOfTheMessage = "A megadott útvonal vagy fájlnév túl hosszú." }
                    }
                },
                { Language.ro, new List<Error>
                    {
                        new Error { Message = "NullReferenceException", TranslationOfTheMessage = "Referința obiectului nu este setată la o instanță a unui obiect." },
                        new Error { Message = "ArgumentException", TranslationOfTheMessage = "A fost furnizat un argument invalid." },
                        new Error { Message = "InvalidOperationException", TranslationOfTheMessage = "Operația nu este valabilă datorită stării actuale." },
                        new Error { Message = "IndexOutOfRangeException", TranslationOfTheMessage = "Indexul a fost în afara limitelor array-ului." },
                        new Error { Message = "DivideByZeroException", TranslationOfTheMessage = "S-a încercat împărțirea la zero." },
                        new Error { Message = "FormatException", TranslationOfTheMessage = "Șirul de intrare nu a fost într-un format corect." },
                        new Error { Message = "InvalidCastException", TranslationOfTheMessage = "Conversia specificată nu este validă." },
                        new Error { Message = "NotImplementedException", TranslationOfTheMessage = "Metoda sau operația nu este implementată." },
                        new Error { Message = "TimeoutException", TranslationOfTheMessage = "Operația a depășit limita de timp." },
                        new Error { Message = "FileNotFoundException", TranslationOfTheMessage = "Fișierul specificat nu a fost găsit." },
                        new Error { Message = "UnauthorizedAccessException", TranslationOfTheMessage = "Accesul la cale a fost refuzat." },
                        new Error { Message = "KeyNotFoundException", TranslationOfTheMessage = "Cheia specificată nu a fost găsită." },
                        new Error { Message = "OutOfMemoryException", TranslationOfTheMessage = "Memoria insuficientă pentru a continua execuția programului." },
                        new Error { Message = "StackOverflowException", TranslationOfTheMessage = "Operația solicitată a cauzat un overflow de stivă." },
                        new Error { Message = "AccessViolationException", TranslationOfTheMessage = "S-a încercat citirea sau scrierea în memorie protejată." },
                        new Error { Message = "SqlException", TranslationOfTheMessage = "A apărut o eroare SQL Server." },
                        new Error { Message = "IOException", TranslationOfTheMessage = "A apărut o eroare I/O." },
                        new Error { Message = "OperationCanceledException", TranslationOfTheMessage = "Operația a fost anulată." },
                        new Error { Message = "ArgumentNullException", TranslationOfTheMessage = "Valoarea nu poate fi null." },
                        new Error { Message = "NotSupportedException", TranslationOfTheMessage = "Operația nu este acceptată." },
                        new Error { Message = "ObjectDisposedException", TranslationOfTheMessage = "Nu se poate accesa un obiect eliminat." },
                        new Error { Message = "ArithmeticException", TranslationOfTheMessage = "A apărut o eroare în timpul operațiunilor aritmetice." },
                        new Error { Message = "DirectoryNotFoundException", TranslationOfTheMessage = "Directorul specificat nu există." },
                        new Error { Message = "PathTooLongException", TranslationOfTheMessage = "Calea sau numele fișierului specificat este prea lung." }
                    }
                },
                { Language.sk, new List<Error>
                    {
                        new Error { Message = "NullReferenceException", TranslationOfTheMessage = "Odkaz na objekt nie je nastavený na inštanciu objektu." },
                        new Error { Message = "ArgumentException", TranslationOfTheMessage = "Bol poskytnutý neplatný argument." },
                        new Error { Message = "InvalidOperationException", TranslationOfTheMessage = "Operácia nie je platná vzhľadom na aktuálny stav." },
                        new Error { Message = "IndexOutOfRangeException", TranslationOfTheMessage = "Index bol mimo medzí poľa." },
                        new Error { Message = "DivideByZeroException", TranslationOfTheMessage = "Snažíte sa deliť nulou." },
                        new Error { Message = "FormatException", TranslationOfTheMessage = "Vstupný reťazec nebol v správnom formáte." },
                        new Error { Message = "InvalidCastException", TranslationOfTheMessage = "Špecifikovaná konverzia typu nie je platná." },
                        new Error { Message = "NotImplementedException", TranslationOfTheMessage = "Metóda alebo operácia nie je implementovaná." },
                        new Error { Message = "TimeoutException", TranslationOfTheMessage = "Operácia prekročila časový limit." },
                        new Error { Message = "FileNotFoundException", TranslationOfTheMessage = "Zadaný súbor nebol nájdený." },
                        new Error { Message = "UnauthorizedAccessException", TranslationOfTheMessage = "Prístup k ceste bol zamietnutý." },
                        new Error { Message = "KeyNotFoundException", TranslationOfTheMessage = "Zadaný kľúč nebol nájdený." },
                        new Error { Message = "OutOfMemoryException", TranslationOfTheMessage = "Nedostatok pamäte na pokračovanie vykonávania programu." },
                        new Error { Message = "StackOverflowException", TranslationOfTheMessage = "Požadovaná operácia spôsobila pretečenie zásobníka." },
                        new Error { Message = "AccessViolationException", TranslationOfTheMessage = "Snažíte sa čítať alebo zapisovať do chránených pamäťových oblastí." },
                        new Error { Message = "SqlException", TranslationOfTheMessage = "Nastala chyba SQL Server." },
                        new Error { Message = "IOException", TranslationOfTheMessage = "Nastala chyba vstupu/výstupu." },
                        new Error { Message = "OperationCanceledException", TranslationOfTheMessage = "Operácia bola zrušená." },
                        new Error { Message = "ArgumentNullException", TranslationOfTheMessage = "Hodnota nemôže byť null." },
                        new Error { Message = "NotSupportedException", TranslationOfTheMessage = "Operácia nie je podporovaná." },
                        new Error { Message = "ObjectDisposedException", TranslationOfTheMessage = "Nie je možné pristupovať k zlikvidovanému objektu." },
                        new Error { Message = "ArithmeticException", TranslationOfTheMessage = "Pri aritmetických operáciách došlo k chybe." },
                        new Error { Message = "DirectoryNotFoundException", TranslationOfTheMessage = "Zadaný adresár neexistuje." },
                        new Error { Message = "PathTooLongException", TranslationOfTheMessage = "Zadaná cesta alebo názov súboru je príliš dlhý." }
                    }
                },
                { Language.bg, new List<Error>
                    {
                        new Error { Message = "NullReferenceException", TranslationOfTheMessage = "Справката към обект не е зададена на инстанция на обект." },
                        new Error { Message = "ArgumentException", TranslationOfTheMessage = "Предоставен е невалиден аргумент." },
                        new Error { Message = "InvalidOperationException", TranslationOfTheMessage = "Операцията не е валидна поради текущото състояние." },
                        new Error { Message = "IndexOutOfRangeException", TranslationOfTheMessage = "Индексът е извън границите на масива." },
                        new Error { Message = "DivideByZeroException", TranslationOfTheMessage = "Опит за деление на нула." },
                        new Error { Message = "FormatException", TranslationOfTheMessage = "Входният низ не е в правилен формат." },
                        new Error { Message = "InvalidCastException", TranslationOfTheMessage = "Посоченото преобразуване не е валидно." },
                        new Error { Message = "NotImplementedException", TranslationOfTheMessage = "Методът или операцията не са реализирани." },
                        new Error { Message = "TimeoutException", TranslationOfTheMessage = "Операцията надхвърли времевия лимит." },
                        new Error { Message = "FileNotFoundException", TranslationOfTheMessage = "Посоченият файл не беше намерен." },
                        new Error { Message = "UnauthorizedAccessException", TranslationOfTheMessage = "Достъпът до пътя е отказан." },
                        new Error { Message = "KeyNotFoundException", TranslationOfTheMessage = "Посоченият ключ не беше намерен." },
                        new Error { Message = "OutOfMemoryException", TranslationOfTheMessage = "Недостатъчно памет за продължаване на изпълнението на програмата." },
                        new Error { Message = "StackOverflowException", TranslationOfTheMessage = "Заявена операция предизвика преливане на стека." },
                        new Error { Message = "AccessViolationException", TranslationOfTheMessage = "Опит за четене или писане в защитена памет." },
                        new Error { Message = "SqlException", TranslationOfTheMessage = "Възникна SQL Server грешка." },
                        new Error { Message = "IOException", TranslationOfTheMessage = "Възникна грешка при вход/изход." },
                        new Error { Message = "OperationCanceledException", TranslationOfTheMessage = "Операцията беше отменена." },
                        new Error { Message = "ArgumentNullException", TranslationOfTheMessage = "Стойността не може да бъде null." },
                        new Error { Message = "NotSupportedException", TranslationOfTheMessage = "Операцията не е поддържана." },
                        new Error { Message = "ObjectDisposedException", TranslationOfTheMessage = "Не може да се получи достъп до освободен обект." },
                        new Error { Message = "ArithmeticException", TranslationOfTheMessage = "Възникна грешка по време на аритметични операции." },
                        new Error { Message = "DirectoryNotFoundException", TranslationOfTheMessage = "Посоченият директория не съществува." },
                        new Error { Message = "PathTooLongException", TranslationOfTheMessage = "Посоченият път или име на файл е твърде дълъг." }
                    }
                },
                { Language.th, new List<Error>
                    {
                        new Error { Message = "NullReferenceException", TranslationOfTheMessage = "การอ้างอิงถึงวัตถุไม่ได้ตั้งค่าให้เป็นอินสแตนซ์ของวัตถุ." },
                        new Error { Message = "ArgumentException", TranslationOfTheMessage = "มีการส่งอาร์กิวเมนต์ที่ไม่ถูกต้อง." },
                        new Error { Message = "InvalidOperationException", TranslationOfTheMessage = "การดำเนินการไม่ถูกต้องเนื่องจากสถานะปัจจุบัน." },
                        new Error { Message = "IndexOutOfRangeException", TranslationOfTheMessage = "ดัชนูอยู่เหนือขอบเขตของอาร์เรย์." },
                        new Error { Message = "DivideByZeroException", TranslationOfTheMessage = "พยายามหารด้วยศูนย์." },
                        new Error { Message = "FormatException", TranslationOfTheMessage = "สตริงนำเข้ามิได้อยู่ในรูปแบบที่ถูกต้อง." },
                        new Error { Message = "InvalidCastException", TranslationOfTheMessage = "การแปลงที่ระบุไม่ถูกต้อง." },
                        new Error { Message = "NotImplementedException", TranslationOfTheMessage = "วิธีการหรือการดำเนินการยังไม่ได้รับการดำเนินการ." },
                        new Error { Message = "TimeoutException", TranslationOfTheMessage = "การดำเนินการได้หมดเวลาลง." },
                        new Error { Message = "FileNotFoundException", TranslationOfTheMessage = "ไม่พบไฟล์ที่ระบุ." },
                        new Error { Message = "UnauthorizedAccessException", TranslationOfTheMessage = "ไม่ได้รับอนุญาตให้เข้าถึงเส้นทาง." },
                        new Error { Message = "KeyNotFoundException", TranslationOfTheMessage = "ไม่พบกุญแจที่ระบุ." },
                        new Error { Message = "OutOfMemoryException", TranslationOfTheMessage = "หน่วยความจำไม่เพียงพอในการดำเนินการโปรแกรม." },
                        new Error { Message = "StackOverflowException", TranslationOfTheMessage = "การดำเนินการที่ร้องขอทำให้เกิดการล débเชิงซ้อน." },
                        new Error { Message = "AccessViolationException", TranslationOfTheMessage = "พยายามอ่านหรือเขียนไปยังหน่วยความจำที่ได้รับการป้องกัน." },
                        new Error { Message = "SqlException", TranslationOfTheMessage = "เกิดข้อผิดพลาด SQL Server." },
                        new Error { Message = "IOException", TranslationOfTheMessage = "เกิดข้อผิดพลาด I/O." },
                        new Error { Message = "OperationCanceledException", TranslationOfTheMessage = "การดำเนินการถูกยกเลิก." },
                        new Error { Message = "ArgumentNullException", TranslationOfTheMessage = "ค่าไม่สามารถเป็น null." },
                        new Error { Message = "NotSupportedException", TranslationOfTheMessage = "การดำเนินการไม่รองรับ." },
                        new Error { Message = "ObjectDisposedException", TranslationOfTheMessage = "ไม่สามารถเข้าถึงวัตถุที่ถูกกำจัด." },
                        new Error { Message = "ArithmeticException", TranslationOfTheMessage = "เกิดข้อผิดพลาดในระหว่างการดำเนินการทางคณิตศาสตร์." },
                        new Error { Message = "DirectoryNotFoundException", TranslationOfTheMessage = "ไม่พบไดเรกทอรีที่ระบุ." },
                        new Error { Message = "PathTooLongException", TranslationOfTheMessage = "เส้นทางหรือชื่อไฟล์ที่ระบุมีความยาวเกินไป." }
                    }
                },
                { Language.vi, new List<Error>
                    {
                        new Error { Message = "NullReferenceException", TranslationOfTheMessage = "Tham chiếu đến đối tượng không được thiết lập thành một phiên bản của đối tượng." },
                        new Error { Message = "ArgumentException", TranslationOfTheMessage = "Đã cung cấp một đối số không hợp lệ." },
                        new Error { Message = "InvalidOperationException", TranslationOfTheMessage = "Hoạt động không hợp lệ do trạng thái hiện tại." },
                        new Error { Message = "IndexOutOfRangeException", TranslationOfTheMessage = "Chỉ số nằm ngoài giới hạn của mảng." },
                        new Error { Message = "DivideByZeroException", TranslationOfTheMessage = "Cố gắng chia cho số không." },
                        new Error { Message = "FormatException", TranslationOfTheMessage = "Chuỗi đầu vào không ở định dạng chính xác." },
                        new Error { Message = "InvalidCastException", TranslationOfTheMessage = "Chuyển đổi đã chỉ định không hợp lệ." },
                        new Error { Message = "NotImplementedException", TranslationOfTheMessage = "Phương thức hoặc hoạt động chưa được triển khai." },
                        new Error { Message = "TimeoutException", TranslationOfTheMessage = "Hoạt động đã vượt quá thời gian cho phép." },
                        new Error { Message = "FileNotFoundException", TranslationOfTheMessage = "Không tìm thấy tệp được chỉ định." },
                        new Error { Message = "UnauthorizedAccessException", TranslationOfTheMessage = "Truy cập vào đường dẫn bị từ chối." },
                        new Error { Message = "KeyNotFoundException", TranslationOfTheMessage = "Không tìm thấy khóa được chỉ định." },
                        new Error { Message = "OutOfMemoryException", TranslationOfTheMessage = "Thiếu bộ nhớ để tiếp tục thực hiện chương trình." },
                        new Error { Message = "StackOverflowException", TranslationOfTheMessage = "Hoạt động yêu cầu đã gây ra tràn ngăn xếp." },
                        new Error { Message = "AccessViolationException", TranslationOfTheMessage = "Cố gắng đọc hoặc ghi vào bộ nhớ được bảo vệ." },
                        new Error { Message = "SqlException", TranslationOfTheMessage = "Đã xảy ra lỗi SQL Server." },
                        new Error { Message = "IOException", TranslationOfTheMessage = "Đã xảy ra lỗi I/O." },
                        new Error { Message = "OperationCanceledException", TranslationOfTheMessage = "Hoạt động đã bị hủy." },
                        new Error { Message = "ArgumentNullException", TranslationOfTheMessage = "Giá trị không thể là null." },
                        new Error { Message = "NotSupportedException", TranslationOfTheMessage = "Hoạt động không được hỗ trợ." },
                        new Error { Message = "ObjectDisposedException", TranslationOfTheMessage = "Không thể truy cập vào đối tượng đã bị giải phóng." },
                        new Error { Message = "ArithmeticException", TranslationOfTheMessage = "Đã xảy ra lỗi trong các phép toán số học." },
                        new Error { Message = "DirectoryNotFoundException", TranslationOfTheMessage = "Không tìm thấy thư mục được chỉ định." },
                        new Error { Message = "PathTooLongException", TranslationOfTheMessage = "Đường dẫn hoặc tên tệp được chỉ định quá dài." }
                    }
                },
                { Language.ms, new List<Error>
                    {
                        new Error { Message = "NullReferenceException", TranslationOfTheMessage = "Rujukan objek tidak ditetapkan kepada instans objek." },
                        new Error { Message = "ArgumentException", TranslationOfTheMessage = "Argumen yang diberikan tidak sah." },
                        new Error { Message = "InvalidOperationException", TranslationOfTheMessage = "Operasi tidak sah disebabkan oleh keadaan semasa." },
                        new Error { Message = "IndexOutOfRangeException", TranslationOfTheMessage = "Indeks berada di luar batas array." },
                        new Error { Message = "DivideByZeroException", TranslationOfTheMessage = "Cuba membahagi dengan sifar." },
                        new Error { Message = "FormatException", TranslationOfTheMessage = "Rantaian input tidak dalam format yang betul." },
                        new Error { Message = "InvalidCastException", TranslationOfTheMessage = "Penukaran yang dinyatakan tidak sah." },
                        new Error { Message = "NotImplementedException", TranslationOfTheMessage = "Kaedah atau operasi tidak dilaksanakan." },
                        new Error { Message = "TimeoutException", TranslationOfTheMessage = "Operasi telah tamat waktu." },
                        new Error { Message = "FileNotFoundException", TranslationOfTheMessage = "Fail yang dinyatakan tidak dapat dijumpai." },
                        new Error { Message = "UnauthorizedAccessException", TranslationOfTheMessage = "Akses ke laluan ditolak." },
                        new Error { Message = "KeyNotFoundException", TranslationOfTheMessage = "Kunci yang dinyatakan tidak dapat dijumpai." },
                        new Error { Message = "OutOfMemoryException", TranslationOfTheMessage = "Memori tidak mencukupi untuk meneruskan pelaksanaan program." },
                        new Error { Message = "StackOverflowException", TranslationOfTheMessage = "Operasi yang diminta menyebabkan penimbunan tumpukan." },
                        new Error { Message = "AccessViolationException", TranslationOfTheMessage = "Cuba membaca atau menulis ke memori yang dilindungi." },
                        new Error { Message = "SqlException", TranslationOfTheMessage = "Ralat SQL Server berlaku." },
                        new Error { Message = "IOException", TranslationOfTheMessage = "Ralat I/O berlaku." },
                        new Error { Message = "OperationCanceledException", TranslationOfTheMessage = "Operasi telah dibatalkan." },
                        new Error { Message = "ArgumentNullException", TranslationOfTheMessage = "Nilai tidak boleh null." },
                        new Error { Message = "NotSupportedException", TranslationOfTheMessage = "Operasi tidak disokong." },
                        new Error { Message = "ObjectDisposedException", TranslationOfTheMessage = "Tidak dapat mengakses objek yang telah dibuang." },
                        new Error { Message = "ArithmeticException", TranslationOfTheMessage = "Ralat berlaku semasa operasi aritmetik." },
                        new Error { Message = "DirectoryNotFoundException", TranslationOfTheMessage = "Direktori yang dinyatakan tidak dapat dijumpai." },
                        new Error { Message = "PathTooLongException", TranslationOfTheMessage = "Laluan atau nama fail yang dinyatakan terlalu panjang." }
                    }
                },
                { Language.ne, new List<Error>
                    {
                        new Error { Message = "NullReferenceException", TranslationOfTheMessage = "वस्तु सन्दर्भलाई कुनै वस्तुमा सेट गरिएको छैन।" },
                        new Error { Message = "ArgumentException", TranslationOfTheMessage = "अवैध तर्क प्रस्तुत गरिएको छ।" },
                        new Error { Message = "InvalidOperationException", TranslationOfTheMessage = "वर्तमान अवस्थाको कारणले गर्दा अपरेसन वैध छैन।" },
                        new Error { Message = "IndexOutOfRangeException", TranslationOfTheMessage = "इन्डेक्स एरेको सीमा बाहिर छ।" },
                        new Error { Message = "DivideByZeroException", TranslationOfTheMessage = "शून्यबाट भाग दिने प्रयास गरियो।" },
                        new Error { Message = "FormatException", TranslationOfTheMessage = "प्रविष्ट गरिएको स्ट्रिङ सही स्वरूपमा छैन।" },
                        new Error { Message = "InvalidCastException", TranslationOfTheMessage = "विशिष्ट कास्ट मान्य छैन।" },
                        new Error { Message = "NotImplementedException", TranslationOfTheMessage = "पद्धति वा अपरेसन कार्यान्वयन गरिएको छैन।" },
                        new Error { Message = "TimeoutException", TranslationOfTheMessage = "अपरेसन समयसीमा पार गर्यो।" },
                        new Error { Message = "FileNotFoundException", TranslationOfTheMessage = "विशिष्ट फाइल फेला पारिएन।" },
                        new Error { Message = "UnauthorizedAccessException", TranslationOfTheMessage = "पथमा पहुँच अस्वीकृत गरिएको छ।" },
                        new Error { Message = "KeyNotFoundException", TranslationOfTheMessage = "विशिष्ट कुञ्जी फेला पारिएन।" },
                        new Error { Message = "OutOfMemoryException", TranslationOfTheMessage = "प्रोग्राम चलाउनको लागि पर्याप्त मेमोरी छैन।" },
                        new Error { Message = "StackOverflowException", TranslationOfTheMessage = "अनुरोध गरिएको अपरेसनले स्ट्याक ओभरफ्लो गर्यो।" },
                        new Error { Message = "AccessViolationException", TranslationOfTheMessage = "सुरक्षित मेमोरीमा पढ्न वा लेख्न प्रयास गरियो।" },
                        new Error { Message = "SqlException", TranslationOfTheMessage = "एक SQL सर्भर त्रुटि उत्पन्न भएको छ।" },
                        new Error { Message = "IOException", TranslationOfTheMessage = "एक I/O त्रुटि उत्पन्न भएको छ।" },
                        new Error { Message = "OperationCanceledException", TranslationOfTheMessage = "अपरेसन रद्द गरिएको छ।" },
                        new Error { Message = "ArgumentNullException", TranslationOfTheMessage = "मान शून्य हुन सक्दैन।" },
                        new Error { Message = "NotSupportedException", TranslationOfTheMessage = "अपरेसन समर्थित छैन।" },
                        new Error { Message = "ObjectDisposedException", TranslationOfTheMessage = "एक नष्ट गरिएको वस्तुमा पहुँच छैन।" },
                        new Error { Message = "ArithmeticException", TranslationOfTheMessage = "गणनात्मक अपरेसनमा त्रुटि भएको छ।" },
                        new Error { Message = "DirectoryNotFoundException", TranslationOfTheMessage = "विशिष्ट निर्देशिका फेला पारिएन।" },
                        new Error { Message = "PathTooLongException", TranslationOfTheMessage = "विशिष्ट पथ वा फाइलको नाम धेरै लामो छ।" }
                    }
                },
                { Language.am, new List<Error>
                    {
                        new Error { Message = "NullReferenceException", TranslationOfTheMessage = "አንድ ነገር ተወስዷል ወደ አንድ ነገር ማለት እንዴት እንደሚያመለክት።" },
                        new Error { Message = "ArgumentException", TranslationOfTheMessage = "የተሳሳተ እንዴት ነገር ይቀበሉ እንዴት በታች በሚያወቅበት ይደርሳሉ።" },
                        new Error { Message = "InvalidOperationException", TranslationOfTheMessage = "እንዴት አካል ይጠብቃል ወይም በአሁኑ ጊዜ ይቀበላል።" },
                        new Error { Message = "IndexOutOfRangeException", TranslationOfTheMessage = "አመለካከት እንደ ንጽህን ወይም ይወዳዳል።" },
                        new Error { Message = "DivideByZeroException", TranslationOfTheMessage = "ወይም በዚያ አይደለም።" },
                        new Error { Message = "FormatException", TranslationOfTheMessage = "አንድ ወይም በአብል አንዳንድ ይሁን።" },
                        new Error { Message = "InvalidCastException", TranslationOfTheMessage = "ወይም በአንደኛ ይወዳዳል።" },
                        new Error { Message = "NotImplementedException", TranslationOfTheMessage = "እንዴት ይተን ወይም ተወዳዳል በታች ይሁን።" },
                        new Error { Message = "TimeoutException", TranslationOfTheMessage = "ወይም በላይ ይወዳዳል።" },
                        new Error { Message = "FileNotFoundException", TranslationOfTheMessage = "ይታወቃል እንዴት ነገር ቢኖር።" },
                        new Error { Message = "UnauthorizedAccessException", TranslationOfTheMessage = "አሁን በሚለው ወይም በማንኛውም ይወዳዳል።" },
                        new Error { Message = "KeyNotFoundException", TranslationOfTheMessage = "አይሁን በዚያ ይጠብቃል።" },
                        new Error { Message = "OutOfMemoryException", TranslationOfTheMessage = "ወይም በማንኛውም ይወዳዳል።" },
                        new Error { Message = "StackOverflowException", TranslationOfTheMessage = "በዚያ አይችልም።" },
                        new Error { Message = "AccessViolationException", TranslationOfTheMessage = "ወይም በአንድ ይታወቃል ወይም በምርጥ እንዴት ይቀበሉ ወይም በሚያመለክት።" },
                        new Error { Message = "SqlException", TranslationOfTheMessage = "አንድ የSQL ማዕከል ተግባር አይደለም።" },
                        new Error { Message = "IOException", TranslationOfTheMessage = "አንድ የI/O ምርጥ ይሁን ወይም አይደለም።" },
                        new Error { Message = "OperationCanceledException", TranslationOfTheMessage = "ወይም በአንድ ይቀበሉ ወይም በማንኛውም ይወዳዳል።" },
                        new Error { Message = "ArgumentNullException", TranslationOfTheMessage = "ወይም በማንኛውም ከላይ ይሁን ወይም በዚያ ይወዳዳል።" },
                } },
                { Language.bn, new List<Error>
                    {
                        new Error { Message = "NullReferenceException", TranslationOfTheMessage = "বস্তু নির্দেশক একটি বস্তুতে সেট করা হয়নি।" },
                        new Error { Message = "ArgumentException", TranslationOfTheMessage = "অবৈধ যুক্তি প্রদান করা হয়েছে।" },
                        new Error { Message = "InvalidOperationException", TranslationOfTheMessage = "বর্তমান অবস্থার কারণে অপারেশন বৈধ নয়।" },
                        new Error { Message = "IndexOutOfRangeException", TranslationOfTheMessage = "সূচক অ্যারের সীমার বাইরেও ছিল।" },
                        new Error { Message = "DivideByZeroException", TranslationOfTheMessage = "শূন্য দ্বারা ভাগ করার চেষ্টা করা হয়েছে।" },
                        new Error { Message = "FormatException", TranslationOfTheMessage = "প্রবিষ্ট স্ট্রিং সঠিক আকারে নয়।" },
                        new Error { Message = "InvalidCastException", TranslationOfTheMessage = "নির্দিষ্ট কাস্ট বৈধ নয়।" },
                        new Error { Message = "NotImplementedException", TranslationOfTheMessage = "পদ্ধতি বা অপারেশন বাস্তবায়িত নয়।" },
                        new Error { Message = "TimeoutException", TranslationOfTheMessage = "অপারেশন সময়সীমা অতিক্রম করেছে।" },
                        new Error { Message = "FileNotFoundException", TranslationOfTheMessage = "নির্দিষ্ট ফাইল পাওয়া যায়নি।" },
                        new Error { Message = "UnauthorizedAccessException", TranslationOfTheMessage = "পথে প্রবেশ নিষিদ্ধ।" },
                        new Error { Message = "KeyNotFoundException", TranslationOfTheMessage = "নির্দিষ্ট কী পাওয়া যায়নি।" },
                        new Error { Message = "OutOfMemoryException", TranslationOfTheMessage = "প্রোগ্রাম চলিয়ে রাখার জন্য পর্যাপ্ত মেমরি নেই।" },
                        new Error { Message = "StackOverflowException", TranslationOfTheMessage = "অনুরোধিত অপারেশন একটি স্ট্যাক ওভারফ্লো ঘটিয়েছে।" },
                        new Error { Message = "AccessViolationException", TranslationOfTheMessage = "সংরক্ষিত মেমরিতে পড়তে বা লিখতে চেষ্টা করা হয়েছে।" },
                        new Error { Message = "SqlException", TranslationOfTheMessage = "একটি SQL সার্ভার ত্রুটি ঘটেছে।" },
                        new Error { Message = "IOException", TranslationOfTheMessage = "একটি I/O ত্রুটি ঘটেছে।" },
                        new Error { Message = "OperationCanceledException", TranslationOfTheMessage = "অপারেশন বাতিল করা হয়েছে।" },
                        new Error { Message = "ArgumentNullException", TranslationOfTheMessage = "মান খালি হতে পারে না।" },
                        new Error { Message = "NotSupportedException", TranslationOfTheMessage = "অপারেশন সমর্থিত নয়।" },
                        new Error { Message = "ObjectDisposedException", TranslationOfTheMessage = "একটি নিষ্ক্রিয় বস্তুতে প্রবেশাধিকার নেই।" },
                        new Error { Message = "ArithmeticException", TranslationOfTheMessage = "গণনা অপারেশনে ত্রুটি ঘটেছে।" },
                        new Error { Message = "DirectoryNotFoundException", TranslationOfTheMessage = "নির্দিষ্ট ডিরেক্টরি পাওয়া যায়নি।" },
                        new Error { Message = "PathTooLongException", TranslationOfTheMessage = "নির্দিষ্ট পথ বা ফাইলের নাম খুব দীর্ঘ।" }
                    }
                },
                { Language.kn, new List<Error>
                    {
                        new Error { Message = "NullReferenceException", TranslationOfTheMessage = "ವಸ್ತು ಸೂಚಕವು ಯಾವುದೋ ವಸ್ತುವಿನ ಉದಾಹರಣೆಗೆ ಹೊಂದಿಸಲಾಗಿಲ್ಲ." },
                        new Error { Message = "ArgumentException", TranslationOfTheMessage = "ಒಂದು ಅಸಾಧಾರಣ ವಾದನೆಯನ್ನು ನೀಡಲಾಗಿದೆ." },
                        new Error { Message = "InvalidOperationException", TranslationOfTheMessage = "ಪ್ರಸ್ತುತ ಸ್ಥಿತಿಯ ಅರ್ಥದಲ್ಲಿ ಕಾರ್ಯಾಚರಣೆ ಅಮಾನ್ಯವಾಗಿದೆ." },
                        new Error { Message = "IndexOutOfRangeException", TranslationOfTheMessage = "ಉಲ್ಲೇಖಕ್ಕಿಂತ ಹೊರಗೆ ಇಂದೆಕ್ಸ್ ಇದೆ." },
                        new Error { Message = "DivideByZeroException", TranslationOfTheMessage = "ಶೂನ್ಯದಲ್ಲಿ ಭಾಗವಿತ್ತಕ್ಕೆ ಪ್ರಯತ್ನಿಸಲಾಗಿದೆ." },
                        new Error { Message = "FormatException", TranslationOfTheMessage = "ನಿವೇಶಿತ ಸ್ಟ್ರಿಂಗ್ ಸರಿಯಾದ ರೂಪದಲ್ಲಿ ಇಲ್ಲ." },
                        new Error { Message = "InvalidCastException", TranslationOfTheMessage = "ನಿರ್ಧಿಷ್ಟವಾದ ಕಾಸ್ಟ್ ಅಮಾನ್ಯವಾಗಿದೆ." },
                        new Error { Message = "NotImplementedException", TranslationOfTheMessage = "ವಿಧಾನ ಅಥವಾ ಕಾರ್ಯಾಚರಣೆಯನ್ನು ನಿರ್ವಹಿಸಲಾಗಿದೆ." },
                        new Error { Message = "TimeoutException", TranslationOfTheMessage = "ಕಾರ್ಯಾಚರಣೆ ಕಾಲ ಮೀರಿಸಿದೆ." },
                        new Error { Message = "FileNotFoundException", TranslationOfTheMessage = "ನಿರ್ಧರಿತ ಫೈಲ್ ಕಂಡುಬಂದಿಲ್ಲ." },
                        new Error { Message = "UnauthorizedAccessException", TranslationOfTheMessage = "ಪಾತೆ ಪ್ರವೇಶ ನಿರಾಕರಿಸಲಾಗಿದೆ." },
                        new Error { Message = "KeyNotFoundException", TranslationOfTheMessage = "ನಿರ್ಧರಿತ ಕೀ ಕಂಡುಬಂದಿಲ್ಲ." },
                        new Error { Message = "OutOfMemoryException", TranslationOfTheMessage = "ಆಹ್ವಾನದ ನಿರ್ವಹಣೆಗೆ ಮೆಮೊರಿ ಕೊರತೆಯಾಗಿದೆ." },
                        new Error { Message = "StackOverflowException", TranslationOfTheMessage = "ಕೋರಿದ ಕಾರ್ಯಾಚರಣೆ ಸ್ಟಾಕ್ ಓವರ್ನುವಾಗಿದೆ." },
                        new Error { Message = "AccessViolationException", TranslationOfTheMessage = "ಸುರಕ್ಷಿತ ಮೆಮೊರಿಯಲ್ಲು ಓದಲು ಅಥವಾ ಬರೆಯಲು ಪ್ರಯತ್ನಿಸಲಾಗಿದೆ." },
                        new Error { Message = "SqlException", TranslationOfTheMessage = "SQL ಸರ್ವರ್ ದೋಷ ಸಂಭವಿಸಿದೆ." },
                        new Error { Message = "IOException", TranslationOfTheMessage = "I/O ದೋಷ ಸಂಭವಿಸಿದೆ." },
                        new Error { Message = "OperationCanceledException", TranslationOfTheMessage = "ಕಾರ್ಯಾಚರಣೆ ರದ್ದುಪಡಿಸಲಾಗಿದೆ." },
                        new Error { Message = "ArgumentNullException", TranslationOfTheMessage = "ಮೌಲ್ಯ ಖಾಲಿಯಲ್ಲ." },
                        new Error { Message = "NotSupportedException", TranslationOfTheMessage = "ಕಾರ್ಯಾಚರಣೆ ಬೆಂಬಲಿಸುವುದಿಲ್ಲ." },
                        new Error { Message = "ObjectDisposedException", TranslationOfTheMessage = "ಕೀಲಕವಿಲ್ಲದ ವಸ್ತುವನ್ನು ಪ್ರವೇಶಿಸಲು ಸಾಧ್ಯವಾಗುತ್ತಿಲ್ಲ." },
                        new Error { Message = "ArithmeticException", TranslationOfTheMessage = "ಗಣಿತ ಕ್ರಿಯೆಗಳಲ್ಲಿ ದೋಷ ಸಂಭವಿಸಿದೆ." },
                        new Error { Message = "DirectoryNotFoundException", TranslationOfTheMessage = "ನಿರ್ಧರಿತ ಡೈರೆಕ್ಟರಿ ಕಂಡುಬಂದಿಲ್ಲ." },
                        new Error { Message = "PathTooLongException", TranslationOfTheMessage = "ನಿರ್ಧರಿತ ಮಾರ್ಗ ಅಥವಾ ಫೈಲ್ ಹೆಸರು ತುಂಬಾ ದೀರ್ಘವಾಗಿದೆ." }
                    }
                },
                { Language.te, new List<Error>
                    {
                        new Error { Message = "NullReferenceException", TranslationOfTheMessage = "వస్తువు సూచిక ఒక వస్తువుకు అమర్చబడలేదు." },
                        new Error { Message = "ArgumentException", TranslationOfTheMessage = "చేరే అర్గుమెంట్ సరైనది కాదు." },
                        new Error { Message = "InvalidOperationException", TranslationOfTheMessage = "ప్రస్తుత స్థితికి అనుగుణంగా ఈ చర్య చెల్లదు." },
                        new Error { Message = "IndexOutOfRangeException", TranslationOfTheMessage = "ఇండెక్స్ పధ్ధతి యొక్క పరిమితికి మించి ఉంది." },
                        new Error { Message = "DivideByZeroException", TranslationOfTheMessage = "సున్నలో విభజించడానికి ప్రయత్నించారు." },
                        new Error { Message = "FormatException", TranslationOfTheMessage = "నివేదిత స్ట్రింగ్ సరైన ఆకృతిలో లేదు." },
                        new Error { Message = "InvalidCastException", TranslationOfTheMessage = "నిర్దేశిత కాస్ట్ సరైనది కాదు." },
                        new Error { Message = "NotImplementedException", TranslationOfTheMessage = "మరియు ఆపరేషన్ అమలు చేయబడలేదు." },
                        new Error { Message = "TimeoutException", TranslationOfTheMessage = "ఆపరేషన్ కాలం ముగిసింది." },
                        new Error { Message = "FileNotFoundException", TranslationOfTheMessage = "నిర్దేశిత ఫైలు కనుగొనబడలేదు." },
                        new Error { Message = "UnauthorizedAccessException", TranslationOfTheMessage = "పాథ్‌కు ప్రవేశం నిషేధించబడింది." },
                        new Error { Message = "KeyNotFoundException", TranslationOfTheMessage = "నిర్దేశిత కీ కనుగొనబడలేదు." },
                        new Error { Message = "OutOfMemoryException", TranslationOfTheMessage = "ప్రోగ్రామ్ కొనసాగించడానికి తగినంత మెమరీ లేదు." },
                        new Error { Message = "StackOverflowException", TranslationOfTheMessage = "అభ్యర్థించిన కార్యకలాపం స్టాక్_OVERFLOW." },
                        new Error { Message = "AccessViolationException", TranslationOfTheMessage = "రక్షిత మెమరీలో చదవడానికి లేదా రాయడానికి ప్రయత్నించారు." },
                        new Error { Message = "SqlException", TranslationOfTheMessage = "SQL సర్వర్ లో పొరపాటు జరిగింది." },
                        new Error { Message = "IOException", TranslationOfTheMessage = "I/O పొరపాటు జరిగింది." },
                        new Error { Message = "OperationCanceledException", TranslationOfTheMessage = "చర్య రద్దు చేయబడింది." },
                        new Error { Message = "ArgumentNullException", TranslationOfTheMessage = "విలువ ఖాళీగా ఉండకూడదు." },
                        new Error { Message = "NotSupportedException", TranslationOfTheMessage = "చర్య మద్దతు ఇవ్వబడలేదు." },
                        new Error { Message = "ObjectDisposedException", TranslationOfTheMessage = "చేతుల విరామ సమయంలో ఉత్పత్తిని యాక్సెస్ చేయలేరు." },
                        new Error { Message = "ArithmeticException", TranslationOfTheMessage = "గణిత కార్యకలాపాల సమయంలో పొరపాటు." },
                        new Error { Message = "DirectoryNotFoundException", TranslationOfTheMessage = "నిర్దేశిత డైరెక్టరీ కనుగొనబడలేదు." },
                        new Error { Message = "PathTooLongException", TranslationOfTheMessage = "నిర్దేశిత మార్గం లేదా ఫైల్ పేరు చాలా పొడవుగా ఉంది." }
                    }
                },
                { Language.ta, new List<Error>
                    {
                        new Error { Message = "NullReferenceException", TranslationOfTheMessage = "உருப்படியின் நோக்கம் உருப்படியின் நிபந்தனைத்தைக் குறிக்கவில்லை." },
                        new Error { Message = "ArgumentException", TranslationOfTheMessage = "சுட்டி குறுக்குவிவரம் தவறானது." },
                        new Error { Message = "InvalidOperationException", TranslationOfTheMessage = "நிலைமையைப் பொருத்தாக செயல்பாடு செல்லுபடியாகாது." },
                        new Error { Message = "IndexOutOfRangeException", TranslationOfTheMessage = "இணைச்சியின் எல்லை மீறியுள்ளது." },
                        new Error { Message = "DivideByZeroException", TranslationOfTheMessage = "சூன்னில் பிளவுபடுத்த முயற்சிக்கப்படுகிறது." },
                        new Error { Message = "FormatException", TranslationOfTheMessage = "உள்ளீட்டு சரம் சரியான வடிவத்தில் இல்லை." },
                        new Error { Message = "InvalidCastException", TranslationOfTheMessage = "குறிக்கப்பட்ட காஸ்ட் செல்லுபடியாகாது." },
                        new Error { Message = "NotImplementedException", TranslationOfTheMessage = "முறை அல்லது செயல்பாடு நடைமுறைப்படுத்தப்படவில்லை." },
                        new Error { Message = "TimeoutException", TranslationOfTheMessage = "செயல்பாடு கால எல்லையை மீறியது." },
                        new Error { Message = "FileNotFoundException", TranslationOfTheMessage = "குறிக்கப்பட்ட கோப்பு காணப்படவில்லை." },
                        new Error { Message = "UnauthorizedAccessException", TranslationOfTheMessage = "பாதைக்கு அணுகல் மறுக்கப்பட்டுள்ளது." },
                        new Error { Message = "KeyNotFoundException", TranslationOfTheMessage = "குறிக்கப்பட்ட விசை காணப்படவில்லை." },
                        new Error { Message = "OutOfMemoryException", TranslationOfTheMessage = "ப்ரோகிராமின் செயலாக்கத்தை தொடர கீழ்க்காணும் நினைவகம் இல்லை." },
                        new Error { Message = "StackOverflowException", TranslationOfTheMessage = "கோரப்பட்ட செயல்பாடு குவியலில் நுழைந்துவிட்டது." },
                        new Error { Message = "AccessViolationException", TranslationOfTheMessage = "பாதுகாக்கப்பட்ட நினைவகம் வாசிக்க அல்லது எழுத முயற்சிக்கப்படுகிறது." },
                        new Error { Message = "SqlException", TranslationOfTheMessage = "SQL சர்வரில் பிழை ஏற்பட்டது." },
                        new Error { Message = "IOException", TranslationOfTheMessage = "I/O பிழை ஏற்பட்டது." },
                        new Error { Message = "OperationCanceledException", TranslationOfTheMessage = "செயல்பாடு நிறுத்தப்பட்டது." },
                        new Error { Message = "ArgumentNullException", TranslationOfTheMessage = "மதிப்பு பூஜ்யமாக இருக்க முடியாது." },
                        new Error { Message = "NotSupportedException", TranslationOfTheMessage = "செயல்பாடு ஆதரிக்கப்படவில்லை." },
                        new Error { Message = "ObjectDisposedException", TranslationOfTheMessage = "விலக்கு செய்யப்பட்ட உருப்படியின் அணுகல் கிடையாது." },
                        new Error { Message = "ArithmeticException", TranslationOfTheMessage = "எண்ணியல் செயல்களில் பிழை ஏற்பட்டது." },
                        new Error { Message = "DirectoryNotFoundException", TranslationOfTheMessage = "குறிக்கப்பட்ட கோப்புறை காணப்படவில்லை." },
                        new Error { Message = "PathTooLongException", TranslationOfTheMessage = "குறிக்கப்பட்ட பாதை அல்லது கோப்புப் பெயர் மிகவும் நீளமாக உள்ளது." }
                    }
                },
                { Language.ml, new List<Error>
                    {
                        new Error { Message = "NullReferenceException", TranslationOfTheMessage = "ഓബ്ജക്റ്റ് റെഫറൻസ് ഒരു ഓബ്ജക്റ്റിന്റെ ഉദാഹരണം ക്രമീകരിച്ചിട്ടില്ല." },
                        new Error { Message = "ArgumentException", TranslationOfTheMessage = "ചെറുതിരി ഗുണനിലവാരത്തിലില്ല." },
                        new Error { Message = "InvalidOperationException", TranslationOfTheMessage = "സാധിക്കുന്ന നിലയിൽ പ്രവർത്തനം സാധുവല്ല." },
                        new Error { Message = "IndexOutOfRangeException", TranslationOfTheMessage = "ഇൻഡക്‌സ് അറയ്‌ലിൻ പുറത്താണ്." },
                        new Error { Message = "DivideByZeroException", TranslationOfTheMessage = "സൂന്നിൽ വിഭജിക്കാൻ ശ്രമിക്കുന്നു." },
                        new Error { Message = "FormatException", TranslationOfTheMessage = "ഇൻപുട്ട് സ്റ്റ്രിങ്ങിന്റെ ഫോർമാറ്റ് ശരിയല്ല." },
                        new Error { Message = "InvalidCastException", TranslationOfTheMessage = "നിർദ്ദിഷ്ട കസ്‌ട് അസാധുവാണ്." },
                        new Error { Message = "NotImplementedException", TranslationOfTheMessage = "മേധോദികം അല്ലെങ്കിൽ പ്രവർത്തനം നടപ്പിലാക്കപ്പെട്ടിട്ടില്ല." },
                        new Error { Message = "TimeoutException", TranslationOfTheMessage = "പ്രവർത്തനം സമയം കടന്നുപോയിരിക്കുന്നു." },
                        new Error { Message = "FileNotFoundException", TranslationOfTheMessage = "നിർദ്ദിഷ്ട ഫയൽ കണ്ടെത്തിയിട്ടില്ല." },
                        new Error { Message = "UnauthorizedAccessException", TranslationOfTheMessage = "പാത്തിലേക്കുള്ള പ്രവേശനം നിരസിച്ചു." },
                        new Error { Message = "KeyNotFoundException", TranslationOfTheMessage = "നിർദ്ദിഷ്ട കീ കണ്ടെത്തിയില്ല." },
                        new Error { Message = "OutOfMemoryException", TranslationOfTheMessage = "പ്രോഗ്രാമിന്റെ നിർവാഹം തുടരാൻ മായി മത് എത്തിച്ചേരുന്നില്ല." },
                        new Error { Message = "StackOverflowException", TranslationOfTheMessage = "അഭ്യർത്ഥിച്ച പ്രവർത്തനം സ്റ്റാക്ക് ഒവർഷൂട്ടിൽ എത്തിച്ചേർന്നുവ." },
                        new Error { Message = "AccessViolationException", TranslationOfTheMessage = "സംരക്ഷിത മെമ്മറിയിൽ വായിക്കാൻ അല്ലെങ്കിൽ എഴുതാൻ ശ്രമിക്കുന്നു." },
                        new Error { Message = "SqlException", TranslationOfTheMessage = "SQL സെർവറിലെ ഒരു പിശക് സംഭവിച്ചു." },
                        new Error { Message = "IOException", TranslationOfTheMessage = "I/O പിശക് സംഭവിച്ചു." },
                        new Error { Message = "OperationCanceledException", TranslationOfTheMessage = "പ്രവർത്തനം റദ്ദാക്കിയിരിക്കുന്നു." },
                        new Error { Message = "ArgumentNullException", TranslationOfTheMessage = "മൂല്യം നൂലായിരിക്കേണ്ട." },
                        new Error { Message = "NotSupportedException", TranslationOfTheMessage = "പ്രവർത്തനം പിന്തുണയ്ക്കുന്നില്ല." },
                        new Error { Message = "ObjectDisposedException", TranslationOfTheMessage = "വിതരണമല്ലാത്ത ഒബ്ജക്റ്റിലേക്ക് പ്രവേശിക്കാൻ കഴിയുന്നില്ല." },
                        new Error { Message = "ArithmeticException", TranslationOfTheMessage = "ആയിരികത്തിൽ പിശക് സംഭവിച്ചു." },
                        new Error { Message = "DirectoryNotFoundException", TranslationOfTheMessage = "നിർദ്ദിഷ്ട ഡയറക്ടറി കണ്ടെത്തിയില്ല." },
                        new Error { Message = "PathTooLongException", TranslationOfTheMessage = "നിർദ്ദിഷ്ട പാത്ത് അല്ലെങ്കിൽ ഫയൽ നാമം വളരെ ദീർഘമാണ്." }
                    }
                },
                { Language.uk, new List<Error>
                    {
                        new Error { Message = "NullReferenceException", TranslationOfTheMessage = "Посилання на об'єкт не вказує на екземпляр об'єкта." },
                        new Error { Message = "ArgumentException", TranslationOfTheMessage = "Передано недійсний аргумент." },
                        new Error { Message = "InvalidOperationException", TranslationOfTheMessage = "Операція не є дійсною в даному стані." },
                        new Error { Message = "IndexOutOfRangeException", TranslationOfTheMessage = "Індекс виходить за межі масиву." },
                        new Error { Message = "DivideByZeroException", TranslationOfTheMessage = "Спроба поділити на нуль." },
                        new Error { Message = "FormatException", TranslationOfTheMessage = "Вхідний рядок має неправильний формат." },
                        new Error { Message = "InvalidCastException", TranslationOfTheMessage = "Вказане перетворення недійсне." },
                        new Error { Message = "NotImplementedException", TranslationOfTheMessage = "Метод або операція не реалізовані." },
                        new Error { Message = "TimeoutException", TranslationOfTheMessage = "Операція перевищила час." },
                        new Error { Message = "FileNotFoundException", TranslationOfTheMessage = "Вказаний файл не знайдено." },
                        new Error { Message = "UnauthorizedAccessException", TranslationOfTheMessage = "Доступ до шляху заборонено." },
                        new Error { Message = "KeyNotFoundException", TranslationOfTheMessage = "Вказаного ключа не знайдено." },
                        new Error { Message = "OutOfMemoryException", TranslationOfTheMessage = "Недостатньо пам'яті для продовження виконання програми." },
                        new Error { Message = "StackOverflowException", TranslationOfTheMessage = "Запитувана операція викликала переповнення стека." },
                        new Error { Message = "AccessViolationException", TranslationOfTheMessage = "Спроба читання або запису в захищену пам'ять." },
                        new Error { Message = "SqlException", TranslationOfTheMessage = "Виникла помилка на сервері SQL." },
                        new Error { Message = "IOException", TranslationOfTheMessage = "Сталася помилка вводу/виводу." },
                        new Error { Message = "OperationCanceledException", TranslationOfTheMessage = "Операцію скасовано." },
                        new Error { Message = "ArgumentNullException", TranslationOfTheMessage = "Значення не може бути нульовим." },
                        new Error { Message = "NotSupportedException", TranslationOfTheMessage = "Операція не підтримується." },
                        new Error { Message = "ObjectDisposedException", TranslationOfTheMessage = "Доступ до звільненого об'єкта неможливий." },
                        new Error { Message = "ArithmeticException", TranslationOfTheMessage = "Сталася помилка під час арифметичних операцій." },
                        new Error { Message = "DirectoryNotFoundException", TranslationOfTheMessage = "Вказаний каталог не знайдено." },
                        new Error { Message = "PathTooLongException", TranslationOfTheMessage = "Вказаний шлях або ім'я файлу занадто довге." }
                    }
                },
                { Language.tt, new List<Error>
                    {
                        new Error { Message = "NullReferenceException", TranslationOfTheMessage = "Объект ссылается на экземпляр объекта." },
                        new Error { Message = "ArgumentException", TranslationOfTheMessage = "Күрсәтелгән аргумент дөрес түгел." },
                        new Error { Message = "InvalidOperationException", TranslationOfTheMessage = "Операция хәзерге хәлдә гамәлдә түгел." },
                        new Error { Message = "IndexOutOfRangeException", TranslationOfTheMessage = "Индекс массив чикләрендә түгел." },
                        new Error { Message = "DivideByZeroException", TranslationOfTheMessage = "Нольгә бүлү тырышылды." },
                        new Error { Message = "FormatException", TranslationOfTheMessage = "Кергән юл дөрес форматта түгел." },
                        new Error { Message = "InvalidCastException", TranslationOfTheMessage = "Билгеләнгән тип дөрес түгел." },
                        new Error { Message = "NotImplementedException", TranslationOfTheMessage = "Метод яки операция гамәлгә ашырылмаган." },
                        new Error { Message = "TimeoutException", TranslationOfTheMessage = "Операция вакыты чыкты." },
                        new Error { Message = "FileNotFoundException", TranslationOfTheMessage = "Күрсәтелгән файл табылмады." },
                        new Error { Message = "UnauthorizedAccessException", TranslationOfTheMessage = "Юлга керү тыелган." },
                        new Error { Message = "KeyNotFoundException", TranslationOfTheMessage = "Күрсәтелгән ачкыч табылмады." },
                        new Error { Message = "OutOfMemoryException", TranslationOfTheMessage = "Программаның эшләтеп җибәрүе өчен җитәрлек хәтер юк." },
                        new Error { Message = "StackOverflowException", TranslationOfTheMessage = "Соралган операция стека өстен чыгуга китерде." },
                        new Error { Message = "AccessViolationException", TranslationOfTheMessage = "Тире үтәлгән хәтердә укырга яки язарга тырышылды." },
                        new Error { Message = "SqlException", TranslationOfTheMessage = "SQL серверында хата булды." },
                        new Error { Message = "IOException", TranslationOfTheMessage = "Керү/чыгу хата булды." },
                        new Error { Message = "OperationCanceledException", TranslationOfTheMessage = "Операция туктатылды." },
                        new Error { Message = "ArgumentNullException", TranslationOfTheMessage = "Бәя нуль була алмый." },
                        new Error { Message = "NotSupportedException", TranslationOfTheMessage = "Операция ярдәм ителми." },
                        new Error { Message = "ObjectDisposedException", TranslationOfTheMessage = "Чыдам объектка мөрәҗәгать итәргә мөмкин түгел." },
                        new Error { Message = "ArithmeticException", TranslationOfTheMessage = "Хисап операцияләре вакытында хата булды." },
                        new Error { Message = "DirectoryNotFoundException", TranslationOfTheMessage = "Күрсәтелгән каталог табылмады." },
                        new Error { Message = "PathTooLongException", TranslationOfTheMessage = "Күрсәтелгән юл яки файл исеме артык озын." }
                    }
                },
                { Language.fa, new List<Error>
                    {
                        new Error { Message = "NullReferenceException", TranslationOfTheMessage = "مرجع به شیء به یک نمونه شیء تنظیم نشده است." },
                        new Error { Message = "ArgumentException", TranslationOfTheMessage = "آرگومان ارائه شده نامعتبر است." },
                        new Error { Message = "InvalidOperationException", TranslationOfTheMessage = "عملیات در وضعیت کنونی معتبر نیست." },
                        new Error { Message = "IndexOutOfRangeException", TranslationOfTheMessage = "اندیس خارج از محدوده آرایه است." },
                        new Error { Message = "DivideByZeroException", TranslationOfTheMessage = "تلاش برای تقسیم بر صفر." },
                        new Error { Message = "FormatException", TranslationOfTheMessage = "رشته ورودی در قالب صحیح نیست." },
                        new Error { Message = "InvalidCastException", TranslationOfTheMessage = "نوع مشخص شده نامعتبر است." },
                        new Error { Message = "NotImplementedException", TranslationOfTheMessage = "متد یا عملیات پیاده‌سازی نشده است." },
                        new Error { Message = "TimeoutException", TranslationOfTheMessage = "عملیات زمان‌بر بود." },
                        new Error { Message = "FileNotFoundException", TranslationOfTheMessage = "فایل مشخص شده پیدا نشد." },
                        new Error { Message = "UnauthorizedAccessException", TranslationOfTheMessage = "دسترسی به مسیر رد شد." },
                        new Error { Message = "KeyNotFoundException", TranslationOfTheMessage = "کلید مشخص شده پیدا نشد." },
                        new Error { Message = "OutOfMemoryException", TranslationOfTheMessage = "حافظه کافی برای ادامه اجرای برنامه موجود نیست." },
                        new Error { Message = "StackOverflowException", TranslationOfTheMessage = "عملیات درخواست شده باعث سرریز پشته شد." },
                        new Error { Message = "AccessViolationException", TranslationOfTheMessage = "تلاش برای خواندن یا نوشتن در حافظه محافظت شده." },
                        new Error { Message = "SqlException", TranslationOfTheMessage = "خطایی در سرور SQL رخ داد." },
                        new Error { Message = "IOException", TranslationOfTheMessage = "خطای ورودی/خروجی رخ داد." },
                        new Error { Message = "OperationCanceledException", TranslationOfTheMessage = "عملیات لغو شد." },
                        new Error { Message = "ArgumentNullException", TranslationOfTheMessage = "مقدار نمی‌تواند null باشد." },
                        new Error { Message = "NotSupportedException", TranslationOfTheMessage = "عملیات پشتیبانی نمی‌شود." },
                        new Error { Message = "ObjectDisposedException", TranslationOfTheMessage = "دسترسی به شیء حذف شده ممکن نیست." },
                        new Error { Message = "ArithmeticException", TranslationOfTheMessage = "خطا در عملیات حسابی." },
                        new Error { Message = "DirectoryNotFoundException", TranslationOfTheMessage = "دایرکتوری مشخص شده پیدا نشد." },
                        new Error { Message = "PathTooLongException", TranslationOfTheMessage = "مسیر یا نام فایل مشخص شده بسیار طولانی است." }
                    }
                },
                { Language.gl, new List<Error>
                    {
                        new Error { Message = "NullReferenceException", TranslationOfTheMessage = "A referencia ao obxecto non está establecida como unha instancia dun obxecto." },
                        new Error { Message = "ArgumentException", TranslationOfTheMessage = "O argumento proporcionado non é válido." },
                        new Error { Message = "InvalidOperationException", TranslationOfTheMessage = "A operación non é válida no estado actual." },
                        new Error { Message = "IndexOutOfRangeException", TranslationOfTheMessage = "O índice está fóra dos límites da matriz." },
                        new Error { Message = "DivideByZeroException", TranslationOfTheMessage = "Intentouse dividir por cero." },
                        new Error { Message = "FormatException", TranslationOfTheMessage = "A cadea de entrada non estaba nun formato correcto." },
                        new Error { Message = "InvalidCastException", TranslationOfTheMessage = "A conversión especificada non é válida." },
                        new Error { Message = "NotImplementedException", TranslationOfTheMessage = "O método ou operación non está implementado." },
                        new Error { Message = "TimeoutException", TranslationOfTheMessage = "A operación excedeu o tempo de espera." },
                        new Error { Message = "FileNotFoundException", TranslationOfTheMessage = "Non se pode atopar o arquivo especificado." },
                        new Error { Message = "UnauthorizedAccessException", TranslationOfTheMessage = "O acceso ao camiño foi denegado." },
                        new Error { Message = "KeyNotFoundException", TranslationOfTheMessage = "Non se atopou a clave especificada." },
                        new Error { Message = "OutOfMemoryException", TranslationOfTheMessage = "Memoria insuficiente para continuar a execución do programa." },
                        new Error { Message = "StackOverflowException", TranslationOfTheMessage = "A operación solicitada causou un desbordamento de pila." },
                        new Error { Message = "AccessViolationException", TranslationOfTheMessage = "Intentouse ler ou escribir en memoria protexida." },
                        new Error { Message = "SqlException", TranslationOfTheMessage = "Produciuse un erro no servidor SQL." },
                        new Error { Message = "IOException", TranslationOfTheMessage = "Produciuse un erro de entrada/salida." },
                        new Error { Message = "OperationCanceledException", TranslationOfTheMessage = "A operación foi cancelada." },
                        new Error { Message = "ArgumentNullException", TranslationOfTheMessage = "O valor non pode ser nulo." },
                        new Error { Message = "NotSupportedException", TranslationOfTheMessage = "A operación non é compatible." },
                        new Error { Message = "ObjectDisposedException", TranslationOfTheMessage = "Non se pode acceder a un obxecto descartado." },
                        new Error { Message = "ArithmeticException", TranslationOfTheMessage = "Produciuse un erro durante as operacións aritméticas." },
                        new Error { Message = "DirectoryNotFoundException", TranslationOfTheMessage = "Non se pode atopar o directorio especificado." },
                        new Error { Message = "PathTooLongException", TranslationOfTheMessage = "A ruta ou o nome do arquivo especificado son demasiado longos." }
                    }
                },
                { Language.ca, new List<Error>
                    {
                        new Error { Message = "NullReferenceException", TranslationOfTheMessage = "La referència a l'objecte no està establerta a una instància d'objecte." },
                        new Error { Message = "ArgumentException", TranslationOfTheMessage = "L'argument proporcionat no és vàlid." },
                        new Error { Message = "InvalidOperationException", TranslationOfTheMessage = "L'operació no és vàlida en l'estat actual." },
                        new Error { Message = "IndexOutOfRangeException", TranslationOfTheMessage = "L'índex està fora dels límits de l'array." },
                        new Error { Message = "DivideByZeroException", TranslationOfTheMessage = "S'ha intentat dividir per zero." },
                        new Error { Message = "FormatException", TranslationOfTheMessage = "La cadena d'entrada no estava en un format correcte." },
                        new Error { Message = "InvalidCastException", TranslationOfTheMessage = "La conversió especificada no és vàlida." },
                        new Error { Message = "NotImplementedException", TranslationOfTheMessage = "El mètode o l'operació no està implementat." },
                        new Error { Message = "TimeoutException", TranslationOfTheMessage = "L'operació ha superat el temps d'espera." },
                        new Error { Message = "FileNotFoundException", TranslationOfTheMessage = "No s'ha pogut trobar el fitxer especificat." },
                        new Error { Message = "UnauthorizedAccessException", TranslationOfTheMessage = "Accés al camí denegat." },
                        new Error { Message = "KeyNotFoundException", TranslationOfTheMessage = "No s'ha trobat la clau especificada." },
                        new Error { Message = "OutOfMemoryException", TranslationOfTheMessage = "No hi ha prou memòria per continuar l'execució del programa." },
                        new Error { Message = "StackOverflowException", TranslationOfTheMessage = "L'operació sol·licitada ha causat un desbordament de pila." },
                        new Error { Message = "AccessViolationException", TranslationOfTheMessage = "S'ha intentat llegir o escriure en memòria protegida." },
                        new Error { Message = "SqlException", TranslationOfTheMessage = "S'ha produït un error al servidor SQL." },
                        new Error { Message = "IOException", TranslationOfTheMessage = "S'ha produït un error d'entrada/sortida." },
                        new Error { Message = "OperationCanceledException", TranslationOfTheMessage = "L'operació ha estat cancel·lada." },
                        new Error { Message = "ArgumentNullException", TranslationOfTheMessage = "El valor no pot ser null." },
                        new Error { Message = "NotSupportedException", TranslationOfTheMessage = "L'operació no és compatible." },
                        new Error { Message = "ObjectDisposedException", TranslationOfTheMessage = "No es pot accedir a un objecte descartat." },
                        new Error { Message = "ArithmeticException", TranslationOfTheMessage = "S'ha produït un error durant les operacions aritmètiques." },
                        new Error { Message = "DirectoryNotFoundException", TranslationOfTheMessage = "No s'ha trobat el directori especificat." },
                        new Error { Message = "PathTooLongException", TranslationOfTheMessage = "La ruta o el nom del fitxer especificat són massa llargs." }
                    }
                },
                { Language.sl, new List<Error>
                    {
                        new Error { Message = "NullReferenceException", TranslationOfTheMessage = "Reference na objekt ni nastavljena na primerka objekta." },
                        new Error { Message = "ArgumentException", TranslationOfTheMessage = "Dani argument je neveljaven." },
                        new Error { Message = "InvalidOperationException", TranslationOfTheMessage = "Operacija ni veljavna v trenutnem stanju." },
                        new Error { Message = "IndexOutOfRangeException", TranslationOfTheMessage = "Indeks je zunaj meja tabele." },
                        new Error { Message = "DivideByZeroException", TranslationOfTheMessage = "Poskus deljenja z nič." },
                        new Error { Message = "FormatException", TranslationOfTheMessage = "Vhodna niz ni v pravilnem formatu." },
                        new Error { Message = "InvalidCastException", TranslationOfTheMessage = "Določena pretvorba ni veljavna." },
                        new Error { Message = "NotImplementedException", TranslationOfTheMessage = "Metoda ali operacija ni implementirana." },
                        new Error { Message = "TimeoutException", TranslationOfTheMessage = "Operacija je potekla." },
                        new Error { Message = "FileNotFoundException", TranslationOfTheMessage = "Določenega datoteke ni mogoče najti." },
                        new Error { Message = "UnauthorizedAccessException", TranslationOfTheMessage = "Dostop do poti je zavrnjen." },
                        new Error { Message = "KeyNotFoundException", TranslationOfTheMessage = "Določena ključna beseda ni bila najdena." },
                        new Error { Message = "OutOfMemoryException", TranslationOfTheMessage = "Pomanjkanje pomnilnika za nadaljevanje izvajanja programa." },
                        new Error { Message = "StackOverflowException", TranslationOfTheMessage = "Zahtevana operacija je povzročila prelivanje sklada." },
                        new Error { Message = "AccessViolationException", TranslationOfTheMessage = "Poskus branja ali pisanja zaščitene pomnilniške." },
                        new Error { Message = "SqlException", TranslationOfTheMessage = "Napaka SQL strežnika." },
                        new Error { Message = "IOException", TranslationOfTheMessage = "Napaka pri vhodu/izhodu." },
                        new Error { Message = "OperationCanceledException", TranslationOfTheMessage = "Operacija je bila preklicana." },
                        new Error { Message = "ArgumentNullException", TranslationOfTheMessage = "Vrednost ne sme biti null." },
                        new Error { Message = "NotSupportedException", TranslationOfTheMessage = "Operacija ni podprta." },
                        new Error { Message = "ObjectDisposedException", TranslationOfTheMessage = "Dostop do izbrisanega objekta ni mogoč." },
                        new Error { Message = "ArithmeticException", TranslationOfTheMessage = "Napaka pri aritmetičnih operacijah." },
                        new Error { Message = "DirectoryNotFoundException", TranslationOfTheMessage = "Določenega imenika ni mogoče najti." },
                        new Error { Message = "PathTooLongException", TranslationOfTheMessage = "Določena pot ali ime datoteke je predolga." }
                    }
                },
                { Language.lt, new List<Error>
                    {
                        new Error { Message = "NullReferenceException", TranslationOfTheMessage = "Objekto nuoroda nenustatyta objekto egzemplioriui." },
                        new Error { Message = "ArgumentException", TranslationOfTheMessage = "Nurodytas argumentas yra neteisingas." },
                        new Error { Message = "InvalidOperationException", TranslationOfTheMessage = "Veiksmas nėra galiojantis esamoje būsenoje." },
                        new Error { Message = "IndexOutOfRangeException", TranslationOfTheMessage = "Indeksas yra už masyvo ribų." },
                        new Error { Message = "DivideByZeroException", TranslationOfTheMessage = "Bandymas dalinti iš nulio." },
                        new Error { Message = "FormatException", TranslationOfTheMessage = "Įvesties eilutė nebuvo tinkamo formato." },
                        new Error { Message = "InvalidCastException", TranslationOfTheMessage = "Nurodyta konversija nėra galiojanti." },
                        new Error { Message = "NotImplementedException", TranslationOfTheMessage = "Metodas arba veiksmas nėra įgyvendintas." },
                        new Error { Message = "TimeoutException", TranslationOfTheMessage = "Veiksmas viršijo laiko limitą." },
                        new Error { Message = "FileNotFoundException", TranslationOfTheMessage = "Nurodytas failas nerastas." },
                        new Error { Message = "UnauthorizedAccessException", TranslationOfTheMessage = "Prieiga prie kelio atmesta." },
                        new Error { Message = "KeyNotFoundException", TranslationOfTheMessage = "Nurodyta raktas nerastas." },
                        new Error { Message = "OutOfMemoryException", TranslationOfTheMessage = "Neužtenka atminties programos vykdymui tęsti." },
                        new Error { Message = "StackOverflowException", TranslationOfTheMessage = "Prašomas veiksmas sukėlė krūvos perpildymą." },
                        new Error { Message = "AccessViolationException", TranslationOfTheMessage = "Bandymas skaityti arba rašyti apsaugotoje atmintyje." },
                        new Error { Message = "SqlException", TranslationOfTheMessage = "SQL Server klaida įvyko." },
                        new Error { Message = "IOException", TranslationOfTheMessage = "Įvesties/išvesties klaida įvyko." },
                        new Error { Message = "OperationCanceledException", TranslationOfTheMessage = "Veiksmas buvo atšauktas." },
                        new Error { Message = "ArgumentNullException", TranslationOfTheMessage = "Vertė negali būti null." },
                        new Error { Message = "NotSupportedException", TranslationOfTheMessage = "Veiksmas nėra palaikomas." },
                        new Error { Message = "ObjectDisposedException", TranslationOfTheMessage = "Negalima pasiekti išmesto objekto." },
                        new Error { Message = "ArithmeticException", TranslationOfTheMessage = "Klaida įvyko aritmetinių operacijų metu." },
                        new Error { Message = "DirectoryNotFoundException", TranslationOfTheMessage = "Nurodyta katalogas nerastas." },
                        new Error { Message = "PathTooLongException", TranslationOfTheMessage = "Nurodyto kelio arba failo pavadinimo ilgis yra per didelis." }
                    }
                },
                { Language.lv, new List<Error>
                    {
                        new Error { Message = "NullReferenceException", TranslationOfTheMessage = "Objekta atsauce nav iestatīta uz objekta eksemplāru." },
                        new Error { Message = "ArgumentException", TranslationOfTheMessage = "Sniedzamais arguments nav derīgs." },
                        new Error { Message = "InvalidOperationException", TranslationOfTheMessage = "Operācija nav derīga pašreizējā stāvoklī." },
                        new Error { Message = "IndexOutOfRangeException", TranslationOfTheMessage = "Indekss ir ārpus masīva robežām." },
                        new Error { Message = "DivideByZeroException", TranslationOfTheMessage = "Mēģinājums dalīt ar nulli." },
                        new Error { Message = "FormatException", TranslationOfTheMessage = "Ievades virkne nebija pareizā formātā." },
                        new Error { Message = "InvalidCastException", TranslationOfTheMessage = "Norādītā pārvēršana nav derīga." },
                        new Error { Message = "NotImplementedException", TranslationOfTheMessage = "Metode vai operācija nav īstenota." },
                        new Error { Message = "TimeoutException", TranslationOfTheMessage = "Operācija ir izpildes laiks." },
                        new Error { Message = "FileNotFoundException", TranslationOfTheMessage = "Norādītais fails netika atrasts." },
                        new Error { Message = "UnauthorizedAccessException", TranslationOfTheMessage = "Piekļuve ceļam ir liegta." },
                        new Error { Message = "KeyNotFoundException", TranslationOfTheMessage = "Norādītais atslēga netika atrasta." },
                        new Error { Message = "OutOfMemoryException", TranslationOfTheMessage = "Atmiņa nav pietiekama programmas izpildes turpināšanai." },
                        new Error { Message = "StackOverflowException", TranslationOfTheMessage = "Pieprasītā operācija izraisīja steka pārplūdi." },
                        new Error { Message = "AccessViolationException", TranslationOfTheMessage = "Mēģinājums lasīt vai rakstīt aizsargātā atmiņā." },
                        new Error { Message = "SqlException", TranslationOfTheMessage = "SQL Server kļūda notika." },
                        new Error { Message = "IOException", TranslationOfTheMessage = "Ievades/izvades kļūda notika." },
                        new Error { Message = "OperationCanceledException", TranslationOfTheMessage = "Operācija tika atcelta." },
                        new Error { Message = "ArgumentNullException", TranslationOfTheMessage = "Vērtībai nedrīkst būt null." },
                        new Error { Message = "NotSupportedException", TranslationOfTheMessage = "Operācija netiek atbalstīta." },
                        new Error { Message = "ObjectDisposedException", TranslationOfTheMessage = "Piekļuve izmestajam objektam nav iespējama." },
                        new Error { Message = "ArithmeticException", TranslationOfTheMessage = "Kļūda notika aritmētisko operāciju laikā." },
                        new Error { Message = "DirectoryNotFoundException", TranslationOfTheMessage = "Norādītā direktorija netika atrasta." },
                        new Error { Message = "PathTooLongException", TranslationOfTheMessage = "Norādītā ceļa vai faila nosaukuma garums ir pārāk liels." }
                    }
                },
                { Language.et, new List<Error>
                    {
                        new Error { Message = "NullReferenceException", TranslationOfTheMessage = "Objekti viide ei ole seadistatud objekti eksemplariks." },
                        new Error { Message = "ArgumentException", TranslationOfTheMessage = "Esitatud argument on vale." },
                        new Error { Message = "InvalidOperationException", TranslationOfTheMessage = "Operatsioon ei ole kehtiv hetke oleku tõttu." },
                        new Error { Message = "IndexOutOfRangeException", TranslationOfTheMessage = "Indeks on massiivi piiridest väljas." },
                        new Error { Message = "DivideByZeroException", TranslationOfTheMessage = "Katse jagada nulliga." },
                        new Error { Message = "FormatException", TranslationOfTheMessage = "Sisendstring ei olnud õiges formaadis." },
                        new Error { Message = "InvalidCastException", TranslationOfTheMessage = "Määratud teisendus ei ole kehtiv." },
                        new Error { Message = "NotImplementedException", TranslationOfTheMessage = "Meetod või operatsioon ei ole rakendatud." },
                        new Error { Message = "TimeoutException", TranslationOfTheMessage = "Operatsioon on aegunud." },
                        new Error { Message = "FileNotFoundException", TranslationOfTheMessage = "Määratud faili ei leitud." },
                        new Error { Message = "UnauthorizedAccessException", TranslationOfTheMessage = "Juurdepääs teele on keelatud." },
                        new Error { Message = "KeyNotFoundException", TranslationOfTheMessage = "Määratud võti ei leitud." },
                        new Error { Message = "OutOfMemoryException", TranslationOfTheMessage = "Mälu ei piisa programmi täitmiseks." },
                        new Error { Message = "StackOverflowException", TranslationOfTheMessage = "Taotletud operatsioon põhjustas virna ülevoolu." },
                        new Error { Message = "AccessViolationException", TranslationOfTheMessage = "Katse lugeda või kirjutada kaitstud mälu." },
                        new Error { Message = "SqlException", TranslationOfTheMessage = "SQL Serveri viga ilmnes." },
                        new Error { Message = "IOException", TranslationOfTheMessage = "Sisend/väljund viga ilmnes." },
                        new Error { Message = "OperationCanceledException", TranslationOfTheMessage = "Operatsioon on tühistatud." },
                        new Error { Message = "ArgumentNullException", TranslationOfTheMessage = "Väärtus ei tohi olla null." },
                        new Error { Message = "NotSupportedException", TranslationOfTheMessage = "Operatsiooni ei toetata." },
                        new Error { Message = "ObjectDisposedException", TranslationOfTheMessage = "Tühi objekt ei ole ligipääsetav." },
                        new Error { Message = "ArithmeticException", TranslationOfTheMessage = "Viga ilmnes aritmeetiliste operatsioonide ajal." },
                        new Error { Message = "DirectoryNotFoundException", TranslationOfTheMessage = "Määratud katalooge ei leitud." },
                        new Error { Message = "PathTooLongException", TranslationOfTheMessage = "Määratud tee või faili nimi on liiga pikk." }
                    }
                },
                { Language.he, new List<Error>
                    {
                        new Error { Message = "NullReferenceException", TranslationOfTheMessage = "הפניה לאובייקט אינה מוגדרת." },
                        new Error { Message = "ArgumentException", TranslationOfTheMessage = "הארגומנט שסופק אינו חוקי." },
                        new Error { Message = "InvalidOperationException", TranslationOfTheMessage = "הפעולה אינה חוקית במצב הנוכחי." },
                        new Error { Message = "IndexOutOfRangeException", TranslationOfTheMessage = "האינדקס מחוץ לגבולות המערך." },
                        new Error { Message = "DivideByZeroException", TranslationOfTheMessage = "ניסיון לחלק באפס." },
                        new Error { Message = "FormatException", TranslationOfTheMessage = "מחרוזת הקלט לא הייתה בפורמט נכון." },
                        new Error { Message = "InvalidCastException", TranslationOfTheMessage = "ההמרה לא חוקית." },
                        new Error { Message = "NotImplementedException", TranslationOfTheMessage = "השיטה או הפעולה לא יושמו." },
                        new Error { Message = "TimeoutException", TranslationOfTheMessage = "הפעולה חרגה מהמגבלה." },
                        new Error { Message = "FileNotFoundException", TranslationOfTheMessage = "הקובץ שצוין לא נמצא." },
                        new Error { Message = "UnauthorizedAccessException", TranslationOfTheMessage = "הגישה לנתיב נדחתה." },
                        new Error { Message = "KeyNotFoundException", TranslationOfTheMessage = "המפתח שצוין לא נמצא." },
                        new Error { Message = "OutOfMemoryException", TranslationOfTheMessage = "אין מספיק זיכרון להמשך הריצה של התוכנית." },
                        new Error { Message = "StackOverflowException", TranslationOfTheMessage = "הפעולה המבוקשת גרמה לגלישת ערימה." },
                        new Error { Message = "AccessViolationException", TranslationOfTheMessage = "ניסיון לקרוא או לכתוב בזיכרון המוגן." },
                        new Error { Message = "SqlException", TranslationOfTheMessage = "שגיאה ב-SQL Server התרחשה." },
                        new Error { Message = "IOException", TranslationOfTheMessage = "שגיאת I/O התרחשה." },
                        new Error { Message = "OperationCanceledException", TranslationOfTheMessage = "הפעולה בוטלה." },
                        new Error { Message = "ArgumentNullException", TranslationOfTheMessage = "הערך לא יכול להיות null." },
                        new Error { Message = "NotSupportedException", TranslationOfTheMessage = "הפעולה אינה נתמכת." },
                        new Error { Message = "ObjectDisposedException", TranslationOfTheMessage = "לא ניתן לגשת לאובייקט שהושלך." },
                        new Error { Message = "ArithmeticException", TranslationOfTheMessage = "שגיאה התרחשה במהלך פעולות חשבון." },
                        new Error { Message = "DirectoryNotFoundException", TranslationOfTheMessage = "הספרייה שצוינה לא קיימת." },
                        new Error { Message = "PathTooLongException", TranslationOfTheMessage = "הנתיב או שם הקובץ שצוינו ארוכים מדי." }
                    }
                },
                { Language.sw, new List<Error>
                    {
                        new Error { Message = "NullReferenceException", TranslationOfTheMessage = "Kurejelea kitu hakijaanzishwa kuwa mfano wa kitu." },
                        new Error { Message = "ArgumentException", TranslationOfTheMessage = "Hoja iliyotolewa si sahihi." },
                        new Error { Message = "InvalidOperationException", TranslationOfTheMessage = "Operesheni haiwezi kufanyika kutokana na hali ya sasa." },
                        new Error { Message = "IndexOutOfRangeException", TranslationOfTheMessage = "Kielekezi kiko nje ya mipaka ya array." },
                        new Error { Message = "DivideByZeroException", TranslationOfTheMessage = "Jaribio la kugawa kwa sifuri." },
                        new Error { Message = "FormatException", TranslationOfTheMessage = "Msimbo wa kuingiza hauko katika muundo sahihi." },
                        new Error { Message = "InvalidCastException", TranslationOfTheMessage = "Kubadilisha kumekosewa." },
                        new Error { Message = "NotImplementedException", TranslationOfTheMessage = "Njia au operesheni haijatekelezwa." },
                        new Error { Message = "TimeoutException", TranslationOfTheMessage = "Operesheni imefika mwisho wa muda." },
                        new Error { Message = "FileNotFoundException", TranslationOfTheMessage = "Faili iliyotajwa haijapatikana." },
                        new Error { Message = "UnauthorizedAccessException", TranslationOfTheMessage = "Ufikiaji wa njia umekataliwa." },
                        new Error { Message = "KeyNotFoundException", TranslationOfTheMessage = "Funguo iliyotajwa haijapatikana." },
                        new Error { Message = "OutOfMemoryException", TranslationOfTheMessage = "Kumbukumbu haiatoshi kuendelea na utekelezaji wa programu." },
                        new Error { Message = "StackOverflowException", TranslationOfTheMessage = "Operesheni iliyotakiwa ilisababisha kujaa kwa rundo." },
                        new Error { Message = "AccessViolationException", TranslationOfTheMessage = "Jaribio la kusoma au kuandika kwenye kumbukumbu iliyolindwa." },
                        new Error { Message = "SqlException", TranslationOfTheMessage = "Kosa la SQL Server limetokea." },
                        new Error { Message = "IOException", TranslationOfTheMessage = "Kosa la I/O limetokea." },
                        new Error { Message = "OperationCanceledException", TranslationOfTheMessage = "Operesheni imeghairiwa." },
                        new Error { Message = "ArgumentNullException", TranslationOfTheMessage = "Thamani haiwezi kuwa null." },
                        new Error { Message = "NotSupportedException", TranslationOfTheMessage = "Operesheni haitekelezwi." },
                        new Error { Message = "ObjectDisposedException", TranslationOfTheMessage = "Haiwezi kufikiwa kwa kitu kilichofutwa." },
                        new Error { Message = "ArithmeticException", TranslationOfTheMessage = "Kosa limetokea wakati wa operesheni za hesabu." },
                        new Error { Message = "DirectoryNotFoundException", TranslationOfTheMessage = "Direktori iliyotajwa haijapatikana." },
                        new Error { Message = "PathTooLongException", TranslationOfTheMessage = "Njia au jina la faili iliyotajwa ni ndefu sana." }
                    }
                },
                { Language.id, new List<Error>
                    {
                        new Error { Message = "NullReferenceException", TranslationOfTheMessage = "Referensi objek tidak diatur ke instansi objek." },
                        new Error { Message = "ArgumentException", TranslationOfTheMessage = "Argumen yang diberikan tidak valid." },
                        new Error { Message = "InvalidOperationException", TranslationOfTheMessage = "Operasi tidak valid karena keadaan saat ini." },
                        new Error { Message = "IndexOutOfRangeException", TranslationOfTheMessage = "Indeks berada di luar batas array." },
                        new Error { Message = "DivideByZeroException", TranslationOfTheMessage = "Mencoba membagi dengan nol." },
                        new Error { Message = "FormatException", TranslationOfTheMessage = "String input tidak dalam format yang benar." },
                        new Error { Message = "InvalidCastException", TranslationOfTheMessage = "Cast yang ditentukan tidak valid." },
                        new Error { Message = "NotImplementedException", TranslationOfTheMessage = "Metode atau operasi belum diimplementasikan." },
                        new Error { Message = "TimeoutException", TranslationOfTheMessage = "Operasi telah melebihi batas waktu." },
                        new Error { Message = "FileNotFoundException", TranslationOfTheMessage = "File yang ditentukan tidak dapat ditemukan." },
                        new Error { Message = "UnauthorizedAccessException", TranslationOfTheMessage = "Akses ke jalur ditolak." },
                        new Error { Message = "KeyNotFoundException", TranslationOfTheMessage = "Kunci yang ditentukan tidak ditemukan." },
                        new Error { Message = "OutOfMemoryException", TranslationOfTheMessage = "Memori tidak cukup untuk melanjutkan eksekusi program." },
                        new Error { Message = "StackOverflowException", TranslationOfTheMessage = "Operasi yang diminta menyebabkan tumpukan meluap." },
                        new Error { Message = "AccessViolationException", TranslationOfTheMessage = "Mencoba membaca atau menulis ke memori yang dilindungi." },
                        new Error { Message = "SqlException", TranslationOfTheMessage = "Terjadi kesalahan SQL Server." },
                        new Error { Message = "IOException", TranslationOfTheMessage = "Terjadi kesalahan I/O." },
                        new Error { Message = "OperationCanceledException", TranslationOfTheMessage = "Operasi telah dibatalkan." },
                        new Error { Message = "ArgumentNullException", TranslationOfTheMessage = "Nilai tidak boleh null." },
                        new Error { Message = "NotSupportedException", TranslationOfTheMessage = "Operasi tidak didukung." },
                        new Error { Message = "ObjectDisposedException", TranslationOfTheMessage = "Tidak dapat mengakses objek yang telah dibuang." },
                        new Error { Message = "ArithmeticException", TranslationOfTheMessage = "Kesalahan terjadi selama operasi aritmatika." },
                        new Error { Message = "DirectoryNotFoundException", TranslationOfTheMessage = "Direktori yang ditentukan tidak ditemukan." },
                        new Error { Message = "PathTooLongException", TranslationOfTheMessage = "Jalur atau nama file yang ditentukan terlalu panjang." }
                    }
                }




            };
        }


        public string GetErrorMessage(Language language, Exception ex)
        {
            string errorKey = ex.GetType().Name;

            // Check if the errorTranslations dictionary has an entry for the given language
            if (!errorTranslations.ContainsKey(language))
            {
                return "No such language found"; // Corrected message
            }

            // Find the error that matches the exception message in the specified language
            var error = errorTranslations[language].FirstOrDefault(e => e.Message == ex.Message);

            // If found, return the translation; otherwise, return the original message
            return error != null ? error.TranslationOfTheMessage : ex.Message;
        }
    }
}
