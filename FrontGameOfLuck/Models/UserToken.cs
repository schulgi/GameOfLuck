using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontGameOfLuck.Models
{
    public class UserToken
    {
        /// <summary>
        /// ID de usuario
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Nombre de usuario
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Nombre completo
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// EMail asociado al usuario
        /// </summary>
        public string EMail { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
