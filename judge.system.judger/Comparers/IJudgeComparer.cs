namespace judge.system.judger.Comparers
{
    public interface IJudgeComparer
    {
        IEnumerable<Issue> Compare(TextReader expected, TextReader real);

    }
}
