# My solutions to Code Jam problems

This repo contains my solutions to a few Google Code Jam problems. I first
wrote these solutions in 2016, and then ported them to F# and wrapped them in a
CLI. They aren't optimal solutions--in fact, one of them is quite slow--but
they are *my* solutions.

# Usage

To run a Code Jam solution, type

```batchfile
CodeJam --problem <problem-name>
```

Like any other Code Jam solution, the input will be taken from stdin, and
output will go to stdout.

To get a usage message, run any of the following

```batchfile
CodeJam
CodeJam -h
CodeJam help
CodeJam --help
```

A usage message is displayed when the arguments are invalid, too.

## Available solutions

| Name            | Link to problem statement                                      |
| --------------- | -------------------------------------------------------------- |
| `rank-and-file` | https://code.google.com/codejam/contest/4304486/dashboard#s=p1 |
| `coin-jam`      | https://code.google.com/codejam/contest/6254486/dashboard#s=p2 |
| `the-last-word` | https://code.google.com/codejam/contest/4304486/dashboard      |

## Possible errors

| Error code | Description                                               |
| ---------- | --------------------------------------------------------- |
| `EARGS`    | The program received an invalid combination of arguments. |

# Reporting issues

Report issues on [GitHub](https://github.com/jmanuel1/code-jam-f-sharp/issues).

# [Code of Conduct](./CODE_OF_CONDUCT.md)

# Acknowledgements

- If you want to learn more about F#, I suggest reading
  [F# for fun and profit](https://fsharpforfunandprofit.com/).
