Before you start writing code, please read this file fully through. It will go over and explain the use of git in relation to this project. 

## 1. Forking the repository

Before writing code you need to make a fork of this repository. This can be done by clicking the fork button on the top right of this page:

![img.png](https://github.com/RubenJ01/KBS-ICTM2C4/blob/main/src/main/resources/readmeImages/img.png?raw=true)

Make sure you select yourself as the owner of the repository. This is done by selecting your own name in the dropdown menu. You can ignore most other options on this page. When you're done click "Create fork" on the bottom of the page:

![img.png](https://github.com/RubenJ01/KBS-ICTM2C4/raw/main/src/main/resources/readmeImages/createFork.png)

## 2. Cloning the repository

Now that you have created your own fork of this repository its time to clone it. Go ahead and navigate to your own Github profile and go to your repositories. Here you should find your newly created fork. Open this repository and navigate to the button "Code" on the top right of this page and copy the URL that's listed next to HTTPS. Make sure your clone your own fork and not the main repository.

![urlKopieëren.png](https://github.com/RubenJ01/KBS-ICTM2C4/blob/main/src/main/resources/readmeImages/urlKopie%C3%ABren.png?raw=true)

![https.png](https://github.com/RubenJ01/KBS-ICTM2C4/blob/main/src/main/resources/readmeImages/https.png?raw=true)

Now go ahead and navigate on your computer to where you want to clone this repository to. Either do this in your command prompt or right click in the desired folder and click "git bash here". 

![gitBashHere.png](https://github.com/RubenJ01/OOSDGroep3/blob/main/readmeImages/gitBashHere.png?raw=true)

This should open up a command prompt like interface. Now you can go ahead and make a clone of the repository with the URL you copied earlier (in order to paste something inside of the command prompt you need to right click). This can be done with the git clone command:

```git
git clone <url>
```

![gitCloneCommand.png](https://github.com/RubenJ01/OOSDGroep3/blob/main/readmeImages/gitCloneCommand.png?raw=true)

Again, make sure this is the git URL of your own fork and not the main repository. 

## 3. Creating your first branch

For every user story we work on we will make a branch on our own fork. This branch is like a new/separate version of your own fork. If you make any errors while working on this branch it will not transfer over to the main branch by default. There is a few important git commands while working with branches:

```git
git branch 
```

Lists all of the branches in your working environment.

 ```git
git branch <name>
 ```

Creates a new branch with the specified name.

```git 
git branch -d <name>
```

This deletes the branch with the name you specified. This is a safe operation and will output and error if that branch has unmerged changes. However if you use an uppercase -D this will delete the branch regardless. Always be careful with using this command.

```git
git checkout <name>
```

Switches you to the branch with the specified name.

Now lets get started by creating your first branch. Make sure your command prompt is in the right directory. You might have to change directories right after cloning:

![changingDirectories.png](https://github.com/RubenJ01/OOSDGroep3/blob/main/readmeImages/changingDirectories.png?raw=true)

To make a new branch lets use the git branch command:

![createBranch.png](https://github.com/RubenJ01/OOSDGroep3/blob/main/readmeImages/createBranch.png?raw=true)

Now lets switch to this branch: 

![gitCheckout.png](https://github.com/RubenJ01/OOSDGroep3/blob/main/readmeImages/gitCheckout.png?raw=true)

## 4. Syncing your fork

Before you start working on new code, its important your fork and branch is synced up. This is important because maybe some code will fundamentally change in the main branch and its important to be up to date with these changes. Gladly Github makes it very easy for us to do this. On GitHub, navigate to the main page of the forked repository that you want to sync with the upstream repository. With upstream we mean where it originated from, so your own fork would be downstream. Select the sync fork dropdown menu in the top right of the page: 

![syncFork.png](https://github.com/RubenJ01/OOSDGroep3/blob/main/readmeImages/syncFork.png?raw=true)

Review the changes and click on update branch. After this make sure you also pull the changes. Do this by switching to your main branch and executing a git pull command:

![gitPull.png](https://github.com/RubenJ01/OOSDGroep3/blob/main/readmeImages/gitPull.png?raw=true)

## 5. Syncing your branch

Now the main branch on your fork is synced up with the main repository. It might also be important to update your fork now with these changes. Get started by switching to your main branch as described earlier with the git checkout command. To do this we can simply merge the main branch into the desired branch with the git merge command, but make sure you are in the right branch when executing this command (so the branch you want to update):

![gitMerge.png](https://github.com/RubenJ01/OOSDGroep3/blob/main/readmeImages/gitMerge.png?raw=true)

## 6. Comitting changes

Now we are ready to start making some changes to the project. For this I've just added a exampe.txt file to my project. Make sure you are on the branch you want to make changes to. Now that I'm ready to merge these changes into upstream (the main repository) I can commit them. But before you can submit something you have to add it first with the git add command:

![gitAdd.png](https://github.com/RubenJ01/OOSDGroep3/blob/main/readmeImages/gitAdd.png?raw=true)

The argument(s) for the git add command are the names of the files you want to add. But if you just want to add every change you can just use a ".".

Next lets commit the changes, we do this with the git commit command, make sure you provide proper details in your commit messages about what you are changing:

![gitCommit.png](https://github.com/RubenJ01/OOSDGroep3/blob/main/readmeImages/gitCommit.png?raw=true)

The syntax for this command is the following:

```git 
git commit -m <commit message>
```

Make sure you wrap your commit message in quotation marks.

Finally, lets push the changes:

![gitPush.png](https://github.com/RubenJ01/OOSDGroep3/blob/main/readmeImages/gitPush.png?raw=true)

## 7. Updating the main branch

To update the main branch we can simply merge our desired branch into the main branch. This is a similar process to the one described in syncing your branch. Get started by switching to your main branch and executing a git merge command with the desired branch. After doing that you will probably encounter this:

![commitMessageMerge.png](https://github.com/RubenJ01/OOSDGroep3/blob/main/readmeImages/commitMessageMerge.png?raw=true)

You can follow the following steps to get out of this window:

1. Press `i` (i for insert).

2. Write your merge message.

3. Press `esc` (escape).

4. Write `:wq` (write & quit).

   This step might give you the following error:

   ![readOnly.png](https://github.com/RubenJ01/OOSDGroep3/blob/main/readmeImages/readOnly.png?raw=true)

   In this case type :wq! to force the merge.

5. Then press enter.

Source: https://stackoverflow.com/questions/19085807/please-enter-a-commit-message-to-explain-why-this-merge-is-necessary-especially

Now you have successfully merged your changes into the main branch, and you should have a similar output to this:

![gitMergeFinished.png](https://github.com/RubenJ01/OOSDGroep3/blob/main/readmeImages/gitMergeFinished.png?raw=true)

Don't forget to now do a git push to push these changes to Github.

## 8. Creating a pull request

The final step in this process is creating a pull request. A pull request is a request to merge your code into the main repository. To do this, navigate to your fork on Github and click on the contribute button:

![img.png](https://github.com/RubenJ01/KBS-ICTM2C4/raw/main/src/main/resources/readmeImages/contribute.png)

Now click on the button "Open pull request". On this page you can review your own changes and add a title and description. Make sure this is a comprehensive description that explains your changes. When you're done just click on the button "Create pull request":

![img.png](https://github.com/RubenJ01/KBS-ICTM2C4/raw/main/src/main/resources/readmeImages/createPr.png)

All that's left to do is wait for other people to review your changes. They might request changes and in that case you can simple commit and push your changes to your main branch and it will update the pull request as well.

