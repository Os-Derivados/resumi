# Resumi Back-end

## Configuração do Ambiente

A aplicação requer as seguintes variáveis de ambiente:

```sh
JWT_SIGNING_KEY=um_hash_128_bytes
JWT_ISSUER=https://<seu-issuer>
JWT_AUDIENCE=https://<sua-audience>
ADMIN_USER={"FullName":"Admin User","PhoneNumber":"+5500000000000","Email":"admin@example.com","Password":"change_me_123!"}
ALLOWED_ORIGIN=http://localhost:3000
CERT_PASSWORD=me-troque
```

Para permitir conexões HTTPS em ambientes Linux, é necessário executar o script `setup-dev-cert.sh`. Este é responsável por:
- Gerar um certificado _self-signed`;
- Guardar uma versão .crt no diretório de certificados do linux, `/etc/pki/ca-trust/source/anchors/`
- Disponibilizar uma vesrão .pfx para uso por parte do `compose.yml`