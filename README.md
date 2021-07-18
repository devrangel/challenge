# Desafio de um contato inteligente (bot).

No projeto foi utilizado a plataforma do Take Blip para implementação do fluxo conversacional.

Também foi utilizado o .NET 5 para o desenvolvimento de uma API que expõe uma única rota GET que retorna os 5 repositórios C# mais antigos em ordem crescente de data de criação.

Primeiro foi implementado o fluxo conversacional utilizando o BUILDER. As imagens foram retiradas do arquivo do fluxo disponibilizado. A imagem dos **carousel component** para o **Fazer desafio** também foi retirada desse mesmo arquivo. No desafio pedia para utilizar o avatar da Take no Github, mas a qualidade da imagem não estava ficando boa, assim foi decidido utilizar a imagem do arquivo do fluxo que ficou melhor.

O segundo passo foi o desenvolvimento da API com o método GET. Foi utilizado o .NET 5. Na pasta **Services** contém o arquivo necessário para, primeiramente, chamar a API do Github e obter todos os repositórios relacionados, e depois retornar para o controller somente os 5 repositórios C# mais antigos. O controller tem apenas um método GET e que tem seu objetivo simples, no qual retorna o objeto com os repositórios ou uma mensagem de **NotFound**.

Terceiro, foi feito o deploy no Heroku. O Heroku não possui suporte para aplicação .NET, mas possui suporte para o Docker. Assim, o deploy foi feito por meio de container.\
Algumas observações precisam ser destacadas em relação ao arquivo Dockerfile.
* É necessário criar outro **user** além do **root**.
* O Heroku faz o mapping das portas automaticamente por meio da variável `<$PORT>`.
* O `<ENTRYPOINT>` deve ser substituido por um `<CMD>`

Exemplo:
```dockerfile
RUN useradd -m chatbotuser
USER chatbotuser

CMD ASPNETCORE_URLS=http://*:$PORT dotnet Api.dll
```

# Executar localmente :
```
> git clone https://github.com/devrangel/challenge.git

> cd challenge/Api/Api

> dotnet run

# No browser acesse:
https://localhost:5001/api/v1/gitdata
```
# Link para aplicação no Heroku
https://chatbotapitakechallenge.herokuapp.com/api/v1/gitdata