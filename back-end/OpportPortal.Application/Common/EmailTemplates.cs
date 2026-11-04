namespace OpportPortal.Application.Common;

public static class EmailTemplates
{
    private static string Wrap(string title, string body) => $@"
<div style='font-family:Arial,sans-serif;max-width:560px;margin:0 auto;border:1px solid #eee;border-radius:12px;overflow:hidden'>
  <div style='background:#1B2A4A;padding:24px;text-align:center'>
    <h1 style='color:#fff;margin:0;font-size:20px'>OpportPortal</h1>
  </div>
  <div style='padding:24px;color:#1B2A4A'>
    <h2 style='color:#1B2A4A;margin-top:0'>{title}</h2>
    {body}
  </div>
  <div style='background:#F7F8FA;padding:16px;text-align:center;color:#9CA3AF;font-size:12px'>
    Portal de Oportunidades Acadêmicas
  </div>
</div>";

    public static (string Subject, string Html) Welcome(string name) =>
        ("Bem-vindo(a) ao OpportPortal! 🎉",
         Wrap("Conta criada com sucesso!",
            $"<p>Olá, {name}!</p><p>Sua conta foi criada. Agora é só completar seu perfil e começar a explorar as oportunidades.</p>"));

    public static (string Subject, string Html) ApplicationCreated(string name, string opportunityTitle) =>
        ("Candidatura enviada ✅",
         Wrap("Candidatura confirmada",
            $"<p>Olá, {name}!</p><p>Sua candidatura para <strong>{opportunityTitle}</strong> foi registrada com sucesso. Boa sorte! 🍀</p>"));

    public static (string Subject, string Html) InterviewScheduled(string name, string opportunityTitle) =>
        ("Você foi convidado(a) para uma entrevista! 📅",
         Wrap("Entrevista agendada",
            $"<p>Olá, {name}!</p><p>A empresa agendou uma <strong>entrevista</strong> para a vaga <strong>{opportunityTitle}</strong>. Fique atento(a) aos próximos contatos.</p>"));

    public static (string Subject, string Html) ApplicationResult(string name, string opportunityTitle, bool approved) =>
        approved
            ? ("Parabéns! Você foi aprovado(a) 🎉",
               Wrap("Candidatura aprovada!",
                  $"<p>Olá, {name}!</p><p>Temos uma ótima notícia: você foi <strong>aprovado(a)</strong> na vaga <strong>{opportunityTitle}</strong>! 🎊</p>"))
            : ("Atualização da sua candidatura",
               Wrap("Resultado da candidatura",
                  $"<p>Olá, {name}!</p><p>Sua candidatura para <strong>{opportunityTitle}</strong> não avançou desta vez. Continue se candidatando — a vaga certa está chegando! 💪</p>"));
}