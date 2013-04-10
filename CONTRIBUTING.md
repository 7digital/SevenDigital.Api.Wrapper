# Contributing

### Overview

* Clone your fork locally and configure the upstream repo:
    `git clone git@github.com:<your-github-username>/SevenDigital.Api.Wrapper`
    `git remote add upstream git://github.com/7digital/SevenDigital.Api.Wrapper`
* Make sure your line-endings are configured correctly:
    `git config core.autocrlf false`
* Create a local branch:
    `git checkout -b my-branch`
* Work on your feature, spiking/prototyping as required
* Rebase as required (see below)
* Push the branch up to GitHub:
    `git push origin my-branch`
* Send a Pull Request on GitHub

You should never work on master, and you should never send a pull request from
master - always from a branch. The reasons for this are detailed below.

### Merging Pull Requests (Notes for 7digital people with push access)

Please try not merge your own pull requests without another team casting their
eye over the request.  Ideally someone from at least 2 teams will review the
request.  This will hopeully ensure we catch any obvious issues before they
make their way into our respective projects and will stimulate a thorough
discussion of any larger refactorings.

There is an internal 7digital api-wrapper google group you can join that will
send you notifications of any changes to the project. We welcome you to join and
pariticipate in discussions and reviews of our pull requests. :)

Usually someone will review your changes within a couple of hours. If not, prod
Greg, someone from the web or locker teams and we will have a look as soon as
we can.


### Guidelines for creating a pull request and notes on style

Following these guidelines will make it much easier for us to appraise and
merge your pull requests.

* Please make sure you follow the [git blessed conventions](http://tbaggery.com/2008/04/19/a-note-about-git-commit-messages.html)
for all your commit messages.
* Please do make sure each individual commit is a discrete piece of functionality
and that each commit builds and passes *all* the tests(!)
* Please make sure that you built with warnings as errors turned on
* Please ensure that all the source files have a green tick in them (resharper
indicator in the top right).
* Please do try and explain why you are suggesting a particular change in as
much detail as possible.  Preferably with real world examples.
*  Fixing major deviations from coding style and whitespace policy is also
encouraged. **Please do make sure you do this in a separate commit as it makes
the history and diffs much easier to follow**

### Basic Git workflow

While you are working away in your branch it is quite possible that your upstream
master may be updated. If this happens you should:

Stash any un-committed changes you need to

    git checkout master
    git pull upstream master
    git checkout my-branch
    git rebase master my-branch
    git push origin master

This ensures that your history is "clean" i.e. you have one branch off from
master followed by your changes in a straight line. Failing to do this ends up
with several "messy" merges in your history, which we would rather avoid.
This is the reason why you should always work in a branch and you should never
be working in or sending pull requests from master.

If you are working on a long running feature then you may want to do this quite
often, rather than run the risk of potential merge issues further down the
line.  When you are ready to go you should confirm that you are up to date and
rebased with upstream/master (see "Handling Updates from Upstream/Master"
above), and then:

    git push origin my-branch

Note that it is important to rebase as described above off master before
asking for a pull request to be merged, we will just ask you to do this anyway
if you have forgotten - make sure you mention this in the pull request in case
people have cloned your fork.

If you have already published your branch in your own fork, you will have to
force push your branch as git (rightly) will warn you that you are about to
rewrite history on a branch that has already been shared.

    git push -f origin my-branch

When you are happy to share the code, send a descriptive Pull Request on GitHub
(see notes above).

Finally, it is perfectly ok and in fact welcomed to submit pull requests on
experimental branches for the purposes of a design discussion or to show off a
spike of some implementation. We only ask that you are clear about this in the
Pull request message.
