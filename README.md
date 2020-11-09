# dotnet Find and Replace Tool 

`dotnet tool find-replace` command

This tool will find and replace strings in your project files.

## Usage

### Install Command

in local

```command
dotnet tool install dotnet-find-replace
```

in global

```command
dotnet tool install --global dotnet-find-replace
```

### Example command

This example replaces
`hello` with `world` in all of your project files.

in local 

```command
dotnet tool run find-replace -- --find "hello" --replace "world"
```

in global

```command
find-replace --find "hello" --replace "world"
```

## Inputs

| Input                  | Description                                                                              |
| ---------------------- | ---------------------------------------------------------------------------------------- |
| `--find`                 | A string to find and replace in your project files. _(This can be a regular expression)_ |
| `--replace`              | The string to replace it with.                                                           |
| `--include` _(optional)_ | A regular expression of files to include. _Defaults to `.*`._                            |
| `--exclude` _(optional)_ | A regular expression of files to exclude. _Defaults to `.git/`._                         |
| `--no-replace`           | no replace file.             |
| `--allow-replace-count-zero` | no replace result is success. |
| `--help`                 | help |
| `--version`              | version |
