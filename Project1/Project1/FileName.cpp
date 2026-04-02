class Ponto {
private:
    // O COFRE: Aqui guardamos as características do Ponto
    float x; // A coordenada X do ponto
    float y; // A coordenada Y do ponto

public:
    // BALCÃO DE ATENDIMENTO: Como interagimos com o Ponto

    // 1. CONSTRUTORES (A equipa de construção)
    Ponto();
    Ponto(float novo_x, float novo_y);

    // 2. GETTERS (Para consultar os valores do cofre)
    float GetX();
    float GetY();

    // 3. SETTERS (Para alterar os valores do cofre)
    void SetX(float novo_x);
    void SetY(float novo_y);
};