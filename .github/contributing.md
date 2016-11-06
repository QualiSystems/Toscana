# Contributing

## Contributing for Quali Employees

- Make sure you are a member of [Toscana Team](https://github.com/orgs/QualiSystems/teams/toscana-team). If not ask organization admins to join the team
- Clone the repository to your local machine using

```bash
$ git clone https://github.com/qualisystems/toscana.git
```

- Create a feature branch using git-flow
```bash
$ git flow feature start name-of-your-feature
```

- Make the required changes:
    - It's important to use to keep high standard of code, so please develop using TDD:
    ### TDD
      - Write a failing test first
      - Write a minimum amount of code to satisfy the failing test
      - Refactor the code

- Commit and push them to github
```bash
$ git add .
$ git commit -m "Your detailed description of your changes"
$ git flow feature end
```
- Submit a pull request. See Pull Request Guidelines below to complete your contribution.

## Contributing for non Quali Employees

- Fork **Toscana** repository to your own GitHub account.
- Clone it to your local machine using

```bash
$ git clone https://github.com/YOUR_ACCOUNT/toscana.git
```

- Create a branch for local development
```bash
$ git checkout -b name-of-your-feature
```

- Make the required changes:
    - It's important to use to keep high standard of code, so please develop using TDD:
    ### TDD
      - Write a failing test first
      - Write a minimum amount of code to satisfy the failing test
      - Refactor the code


- Commit and push them to github
```bash
$ git add .
$ git commit -m "Your detailed description of your changes"
$ git push origin name-of-your-feature
```
- Submit a pull request

## Pull Request Guidelines

Before you submit a pull request, make sure it meets these guidelines:

1. The pull request should include tests that cover the new functionality of the fixed bug.
2. XML documentaiton should be added or updated on public methods, classes, exceptions and interfaces.
3. It's required to test for backward compatability with existing build in TeamCity


Return to [Table of Contents](readme.md)
