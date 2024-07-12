import { Aluno } from "./aluno";

export interface Tarefa {
  imcId?: string;
  altura: number;
  peso: number;
  valor?: number;
  classificacao?: string;
  alunoId?: string;
  aluno?: Aluno;
  dataCriacao?: string;
}
