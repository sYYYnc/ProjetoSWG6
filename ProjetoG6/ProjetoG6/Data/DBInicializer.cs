using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ProjetoG6.Models;
using ProjetoG6.Models.AccountViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoG6.Data
{
    public class DBInicializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            // context.Database.EnsureCreated();
            InitPaisesDB(context);//tabela paises
            InitProgramaMobilidade(context);//tabela programa mobilidade
            InitProgramaMobilidadePaises(context);//tabela programaMobilidadePais
            InitHelp(context);
        }

        /*
         * Metodo que faz insert de dados default na tabela Paises
         */
        private static void InitPaisesDB(ApplicationDbContext context) {
            if (context.Paises.Any())
            {
                return;   //ja tem dados
            }
            var pais = new Paises[]
            {
                    new Paises {Pais="Espanha"},
                    new Paises {Pais="Franca"},
                    new Paises {Pais="Brasil"},
                    new Paises {Pais="Alemanha"},
                    new Paises {Pais="Polonia"},
                    new Paises {Pais="Russia"},
                    new Paises {Pais="Espanha"},
            };

            foreach (Paises s in pais)
            {
                context.Paises.Add(s);
            }
            context.SaveChanges();
        }

        /**
         * Metodo que faz insert de dados default na tabela Programa Moblidade
        */
        private static void InitProgramaMobilidade(ApplicationDbContext context)
        {
            if (context.ProgramaMobilidade.Any())
            {
                return;   //ja tem dados
            }
            var progMobi= new ProgramaMobilidade[]
            {
                    new ProgramaMobilidade {Nome="Programa 1"},
                     new ProgramaMobilidade {Nome="Programa 2"},
                      new ProgramaMobilidade {Nome="Programa 3"},
                       new ProgramaMobilidade {Nome="Programa 4"},
                        new ProgramaMobilidade {Nome="Programa 5"}
            };

            foreach (ProgramaMobilidade prog in progMobi)
            {
                context.ProgramaMobilidade.Add(prog);
            }
            context.SaveChanges();
        }

        /**
        * Metodo que faz insert de dados default na tabela Programa Moblidade Paises
       */
        private static void InitProgramaMobilidadePaises(ApplicationDbContext context)
        {
            if (context.ProgramaMobilidadePais.Any())
            {
                return;   //ja tem dados
            }
            var progMobi = new ProgramaMobilidadePais[]
            {
                //Programa mobilidade 1
                 new ProgramaMobilidadePais{
                     ProgramaMobilidadeID=1,
                     PaisID=1
                    },
                 new ProgramaMobilidadePais{
                     ProgramaMobilidadeID=1,
                     PaisID=2
                    },
                 new ProgramaMobilidadePais{
                     ProgramaMobilidadeID=1,
                     PaisID=3
                    },
                  new ProgramaMobilidadePais{
                     ProgramaMobilidadeID=1,
                     PaisID=5
                    },
                 new ProgramaMobilidadePais{
                     ProgramaMobilidadeID=1,
                     PaisID=6
                    }
            };

            foreach (ProgramaMobilidadePais prog in progMobi)
            {
                context.ProgramaMobilidadePais.Add(prog);
            }
            context.SaveChanges();
        }

        private static void InitHelp(ApplicationDbContext context){
           if (context.Help.Any())
            {
                return;   //ja tem dados
            } 
            var help = new Help[]
            {
                
                 new Help{
                     Campo = "Email",
                     Descricao = "ex: 123456789@estudantes.ips.pt"
                    },
                 new Help{
                     Campo = "Nome",
                     Descricao = "ex: Carlos Ribeiro"
                    },
                 new Help{
                     Campo = "Password",
                     Descricao = "Deverá conter no minimo 8 caracteres"
                    },
                  new Help{
                     Campo = "PasswordConf",
                     Descricao = "Deverá ser igual à definida anteriormente"
                    },
                 new Help{
                     Campo = "NumeroAluno",
                     Descricao = "Deverá ser composto por 9 algarismos (ex: 150221037)"
                    },
                  new Help{
                     Campo = "DataNascimento",
                     Descricao = "ex: 13/03/1997 (13 de Março de 1997)"
                    },
                   new Help{
                     Campo = "HighRegisto",
                     Descricao = "Na pagina -Registo- o utilizador deverá introduzir informação como o seu Nome, Número de Identificação no" +
                     " estabelecimento de ensino, Email, Data de Nascimento, Password e Confirmação da Password. Depois dos dados validados" +
                     " e confirmada a intenção de registo, o sistema enviará um email para que este processo fique concluido."
                    },
                    new Help{
                     Campo = "HighLogin",
                     Descricao = "Na pagina -Login- o utilizador deverá introduzir informação como o seu Email, Password e Confirmação da Password." +
                     " De Seguida, e com os dados já validados a aplicação permite que o utilizador tenha acesso a um conjunto de funcionalidades" +
                     " relativas ao processo de mobilidade."
                    },
                    new Help{
                     Campo = "HighRecuperacaoPass",
                     Descricao = "A página -Recuperação de Password- permite que o utilizador, fornecendo o seu email, Nova Password e Confirmação" +
                     " da nova Password altere a sua password."
                    },
                    new Help{
                     Campo = "HighEsqueceuPass",
                     Descricao = "A página -Esqueceu a Password- permite que o utilizador, fornecendo o seu Email, seja reencaminhado para a página" +
                     " -Recuperação de Password-. O sistema enviará um Email para o utilizador e este, ao abrir o link que lhe é facultado dará" +
                     " entrada na página de recuperação."
                    }
            };

            foreach (Help h in help)
            {
                context.Help.Add(h);
            }
            context.SaveChanges();

        }


    }
}
